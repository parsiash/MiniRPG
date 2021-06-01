using System.Collections.Generic;
using MiniRPG.Battle;
using MiniRPG.BattleLogic;
using MiniRPG.Common;

namespace MiniRPG.BattleView
{
    /// <summary>
    /// Responsible for all battle-related visuals.
    /// </summary>
    public interface IBattleView
    {
        void Init(
            IBattleSimulation battleSimulation, 
            IEntityViewFactory entityViewFactory, 
            IBattleActionListener battleActionListener
        );

        IEnumerable<IEntityView> EntityViews { get; }
        IEntityView GetEntityView(int entityId);
        void Clear();
    }

    public static class BattleViewExtensions
    {
        public static IUnitView GetUnitView(this IBattleView battleView, int entityId)
        {
            return battleView.GetEntityView(entityId) as IUnitView;
        }
    }

    public class BattleView : CommonBehaviour, IBattleView, IEntityViewEventListener
    {
        private IBattleSimulation _battleSimulation;
        private IEntityViewFactory _entityViewFactory;
        private IBattleActionListener _battleActionListener;

        /// <summary>
        /// Entity Views.
        /// </summary>
        public IEnumerable<IEntityView> EntityViews => entityViews.Values;
        private Dictionary<int, IEntityView> _entityViews;
        protected Dictionary<int, IEntityView> entityViews
        {
            get
            {
                _entityViews = _entityViews ?? new Dictionary<int, IEntityView>();

                return _entityViews;
            }
        }

        public void Init(IBattleSimulation battleSimulation, IEntityViewFactory entityViewFactory, IBattleActionListener battleActionListener)
        {
            Clear();

            _battleSimulation = battleSimulation;
            _entityViewFactory = entityViewFactory;
            _battleActionListener = battleActionListener;

            //create entity views
            foreach(var entity in battleSimulation.Entities)
            {
                var entityView = CreateEntityView(entity);
            }
        }

        private IEntityView CreateEntityView(Entity entity)
        {
            //check for entity id conflict
            if(entityViews.ContainsKey(entity.id))
            {
                logger.LogError($"Cannot create entity view. An entity view with the same id : {entity.id} already exists");
                return null;
            }

            var entityView = _entityViewFactory.CreateEntityView("UnitView");
            if(entityView != null)
            {
                entityView.Init(entity, this);
                entityViews.Add(entity.id, entityView);

                logger.LogDebug($"Entity View with name : {entity.name} and id : {entity.id} created and added to battle view.");
            }

            return entityView;
        }

        public IEntityView GetEntityView(int entityId)
        {
            if(entityViews.TryGetValue(entityId, out var entityView))
            {
                return entityView;
            }

            return null;
        }

        private void DestroyEntityView(IEntityView entityView)
        {
            _entityViewFactory.DestroyEntityView(entityView);
        }

        public void Clear()
        {
            foreach(var entityView in entityViews.Values)
            {
                DestroyEntityView(entityView);
            }

            entityViews.Clear();
        }

        public void OnClick(IEntityView entityView)
        {
            var unitView = entityView as UnitView;
            if(unitView)
            {
                _battleActionListener.OnRandomAttack(
                    unitView.Unit.PlayerIndex,
                    unitView.Unit.id
                );
            }
        }
    }
}