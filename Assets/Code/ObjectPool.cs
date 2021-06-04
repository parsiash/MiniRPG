using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniRPG
{
    public interface IPoolable
    {
        string PrefabId { get; }
        void OnCreated(string prefabId);
        void OnBeforePooled();
        void OnAfterRetrieved();
    }

    public class ObjectPool<T> where T : UnityEngine.Object, IPoolable
    {
        private static ObjectPool<T> instance;
        public static ObjectPool<T> Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ObjectPool<T>();
                }

                return instance;
            }
        }

        private Transform _pooledObjectsRoot;
        private Transform pooledObjectsRoot
        {
            get
            {
                if(!_pooledObjectsRoot)
                {
                    _pooledObjectsRoot = new GameObject($"ObjectPool_{typeof(T).Name}").transform;
                    GameObject.DontDestroyOnLoad(_pooledObjectsRoot.gameObject);
                }

                return _pooledObjectsRoot;
            }
        }

        private Dictionary<string, T> _prefabs;
        private Dictionary<string, T> prefabs
        {
            get
            {
                _prefabs = _prefabs ?? new Dictionary<string, T>();
                return _prefabs;
            }
        }

        private Dictionary<string, List<T>> _instancePool;
        private Dictionary<string, List<T>> instancePool
        {
            get
            {
                _instancePool = _instancePool ?? new Dictionary<string, List<T>>();
                return _instancePool;
            }
        }

        public bool HasPrefab(string prefabId)
        {
            return prefabs.ContainsKey(prefabId) && prefabs[prefabId];
        }

        public void AddPrefab(string prefabId, T prefab)
        {
            if(!HasPrefab(prefabId))
            {
                prefabs[prefabId] = prefab;
            }
        }

        public void RemovePrefab(string prefabId)
        {
            if(HasPrefab(prefabId))
            {
                prefabs.Remove(prefabId);
            }
        }

        /// <summary>
        /// Returns the prefab with prefabId.
        /// </summary>
        public T GetPrefab(string prefabId)
        {
            if(!prefabs.ContainsKey(prefabId))
            {
                return null;
            }

            var prefab = prefabs[prefabId];
            if(!prefab)
            {
                prefabs.Remove(prefabId);
                return null;
            }

            return prefab;
        }

        private List<T> GetInstanceListByPrefabId(string prefabId)
        {
            if(!instancePool.ContainsKey(prefabId))
            {
                instancePool[prefabId] = new List<T>();
            }

            return instancePool[prefabId];
        }

        private void CreateInstance(string prefabId)
        {
            var prefab = GetPrefab(prefabId);
            if(prefab)
            {
                CreateInstance(prefabId, GetPrefab(prefabId));
            }
        }

        private void CreateInstance(string prefabId, T prefab)
        {           
            var instance = GameObject.Instantiate<T>(prefab);
            instance.OnCreated(prefabId);
            PoolInstance(prefabId, instance);
        }

        public bool TryPoolInstance(string prefabId, T instance)
        {
            if(!instance || instance.PrefabId != prefabId)
            {
                return false;
            }

            PoolInstance(prefabId, instance);
            return true;
        }

        private void PoolInstance(string prefabId, T instance)
        {
            if(!instance || instance.PrefabId != prefabId)
            {
                return;
            }

            instance.OnBeforePooled();

            //deactivate and parent the MonoBehaviour instances
            var instanceBehaviour = instance as MonoBehaviour;
            if(instanceBehaviour)
            {
                instanceBehaviour.transform.SetParent(pooledObjectsRoot);
                instanceBehaviour.gameObject.SetActive(false);
            }

            GetInstanceListByPrefabId(prefabId).Add(instance);
        }

        public T RetrieveInstance(string prefabId, bool active = false)
        {
            if(!HasPrefab(prefabId))
            {
                return null;
            }
            
            var instanceList = GetInstanceListByPrefabId(prefabId);
            if(instanceList.Count == 0)
            {
                CreateInstance(prefabId);
            }

            if(instanceList.Count > 0)
            {
                var instance = instanceList[0];
                instanceList.RemoveAt(0);

                var instanceBehaviour = instance as MonoBehaviour;
                if(instanceBehaviour)
                {
                    instanceBehaviour.transform.SetParent(null);
                    instanceBehaviour.gameObject.SetActive(active);
                }

                instance.OnAfterRetrieved();

                return instance;
            }

            return null;
        }

        public int GetInstanceCount(string prefabId)
        {
            if(!HasPrefab(prefabId))
            {
                return 0;
            }

            var instanceList = GetInstanceListByPrefabId(prefabId);
            return instanceList.Count;
        }

        public void PrefallocateInstance(string prefabId, int count)
        {
            var instanceCount = GetInstanceCount(prefabId);

            for(int i = instanceCount; i < count; i++)
            {
                CreateInstance(prefabId);
            }
        }
    }
}