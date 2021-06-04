namespace MiniRPG.Common
{
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
