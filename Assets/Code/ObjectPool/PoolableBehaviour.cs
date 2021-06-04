namespace MiniRPG.Common
{
    /// <summary>
    /// The common base class of behaviours that can be used in ObjectPool.
    /// </summary>
    public class PoolableBehaviour : CommonBehaviour, IPoolable
    {
        public string PrefabId { get; set; }

        public virtual void OnCreated(string prefabId)
        {
            PrefabId = prefabId;
        }

        public virtual void OnBeforePooled()
        {
        }

        public virtual void OnAfterRetrieved()
        {
        }
    }
}
