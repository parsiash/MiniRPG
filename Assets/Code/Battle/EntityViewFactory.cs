using MiniRPG.Common;
using UnityEngine;

namespace MiniRPG.BattleView
{
    public interface IEntityViewFactory
    {
        IEntityView CreateEntityView(string name);
        void DestroyEntityView(IEntityView entityView);
    }

    public class EntityViewFactory : IEntityViewFactory
    {
        private Common.ILogger _logger;

        public EntityViewFactory(Common.ILogger logger)
        {
            _logger = logger;
        }

        public IEntityView CreateEntityView(string name)
        {
            //retrive the prefab
            var prefab = Resources.Load<GameObject>("Entities/" + name);
            if(!prefab)
            {
                _logger.LogError($"Cannot create entity view with name : {name}. No Entity View Found.");
            }

            //instantiate the entity view
            var entityObject = GameObject.Instantiate(prefab);
            return entityObject.GetComponent<IEntityView>();
        }

        public void DestroyEntityView(IEntityView entityView)
        {
            if(entityView is CommonBehaviour)
            {
                GameObject.Destroy((entityView as CommonBehaviour).gameObject);
            }
        }
    }
}