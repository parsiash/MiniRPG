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
            var entityViewObject = Resources.Load("Entities/" + name);
            if(!entityViewObject)
            {
                _logger.LogError($"Cannot create entity view with name : {name}. No Entity View Found.");
            }

            return entityViewObject as IEntityView;
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