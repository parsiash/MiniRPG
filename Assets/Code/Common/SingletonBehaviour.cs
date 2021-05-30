using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniRPG.Common
{
    public abstract class SingletonBehaviour : CommonBehaviour
    {

    }

    /// <summary>
    /// A Generic singleton which can be used for any MonoBehaviour.
    /// For making a MonoBehaviour T singleton, just make it inherit from SingletonBehaviour<T>.
    /// Example : GameManager
    /// </summary>
    public class SingletonBehaviour<T> : SingletonBehaviour where T : SingletonBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if(!instance)
                {
                    SetInstance(new GameObject().AddComponent<T>());
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            var component = this as T;
            if(!component)
            {
                DestroyImmediate(gameObject);
                return;
            }

            if(instance && instance != component)
            {
                DestroyImmediate(gameObject);
            }else
            {
                if(instance != component)
                {
                    SetInstance(component);
                }

                Init();
            }
        }

        protected virtual void Init()
        {
            DefaultLogger.Instance.Log($"Singleton Behaviour of type {typeof(T).Name} initialized.");
        }

        private static void SetInstance(T argInstance)
        {
            //first delete all other instances (if there are any)
            var instanceObjects = Object.FindObjectsOfType(typeof(T), true);
            var instances = instanceObjects.Where(io => io != null).Select(io => io as T);
            foreach(var inst in instances)
            {
                if(inst && inst != argInstance)
                {
                    DestroyImmediate(inst.gameObject);
                }
            }

            instance = argInstance;
            instance.gameObject.name = $"Singleton_{typeof(T).Name}";
            GameObject.DontDestroyOnLoad(instance.gameObject);
        }

    }
}
