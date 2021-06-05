using System.Collections.Generic;
using MiniRPG.Battle;
using MiniRPG.Logic.Battle;
using MiniRPG.Common;
using UnityEngine;

namespace MiniRPG.BattleView
{
    /// <summary>
    /// The interface for visual representation of battle simulation.
    /// </summary>
    public interface IBattleView
    {
        void Init(
            IBattleSimulation battleSimulation, 
            IUnitViewFactory entityViewFactory, 
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

    /// <summary>
    /// Visual representation of battle simulation.
    /// </summary>
    public class BattleView : CommonBehaviour, IBattleView, IEntityViewEventListener
    {
        private IBattleSimulation _battleSimulation;
        private IUnitViewFactory _unitViewFactory;
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

        [SerializeField] private Transform[] unitPositions;

        public void Init(IBattleSimulation battleSimulation, IUnitViewFactory unitViewFactory, IBattleActionListener battleActionListener)
        {
            Clear();

            _battleSimulation = battleSimulation;
            _unitViewFactory = unitViewFactory;
            _battleActionListener = battleActionListener;

            //create entity views
            foreach(var entity in battleSimulation.Entities)
            {
                if(entity is Unit)
                {
                    CreateUnitView(entity as Unit);
                }else
                {
                    logger.LogError($"Entity type : {entity.GetType().Name} currently not supported in battle view");
                }
            }
        }

        private IEntityView CreateUnitView(Unit unit)
        {
            //check for entity id conflict
            if (entityViews.ContainsKey(unit.id))
            {
                logger.LogError($"Cannot create unit view. A unit view with the same id : {unit.id} already exists");
                return null;
            }

            //create and intialize the unit view
            var unitView = _unitViewFactory.CreateUnitView("UnitView");
            unitView.Init(unit, this);
            SetUnitViewPosition(unitView);

            entityViews.Add(unit.id, unitView);

            return unitView;
        }

        private void SetUnitViewPosition(IUnitView unitView)
        {
            var player = unitView.Unit.player;
            var unitIndexInTeam = player.GetUnitIndexById(unitView.Unit.id);
            if (unitIndexInTeam >= 0 && unitIndexInTeam < unitPositions.Length)
            {
                var position = unitPositions[unitIndexInTeam].position;
                if (player.index == 1)
                {
                    position.x = -position.x;
                }

                unitView.Position = position;
            }
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
            if(entityView is IUnitView)
            {
                _unitViewFactory.DestroyUnitView(entityView as IUnitView);
            }
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
                _battleActionListener.OnUnitViewClick(unitView);
            }
        }

        public void OnHold(IEntityView entityView)
        {
            if(entityView is IUnitView)
            {
                _battleActionListener.OnUnitViewHold(entityView as IUnitView);
            }
        }

        public void OnEntityDestroy(IEntityView entityView)
        {
            //remove from entity view collection
            var entityId = entityView.Entity.id;
            if(_entityViews.ContainsKey(entityId))
            {
                _entityViews.Remove(entityId);
            }

            DestroyEntityView(entityView);
        }
    }
}