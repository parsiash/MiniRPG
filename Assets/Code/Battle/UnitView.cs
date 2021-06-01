using System;
using MiniRPG.BattleLogic;
using MiniRPG.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniRPG.BattleView
{
    public interface IEntityViewEventListener
    {
        void OnClick(IEntityView entityView);
    }

    public interface IUnitView : IEntityView
    {
        int Health { get; set; }
        void TakeDamage(int damage);
        void Attack(IUnitView target, System.Action OnHit);
    }

    public class UnitView : CommonBehaviour, IUnitView, IPointerClickHandler
    {
        private int _health;
        public int Health
        {
            get
            {
                return _health;
            }

            set
            {
                _health = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                var currentPos = transform.position;
                transform.position = new Vector3(value.x, value.y, currentPos.z);
            }
        }

        private Unit _unit;
        public Entity Entity => _unit;
        public Unit Unit => _unit;
        private IEntityViewEventListener _eventListener;

        public void Init(Entity entity, IEntityViewEventListener eventListener)
        {
            if(entity is Unit)
            {
                _unit = entity as Unit;
                _health = _unit.healthComponent.health;
                
                _eventListener = eventListener;
            }else
            {
                logger.LogError($"Cannot init unit view : {gameObject.name}. The given entity is not a unit entity.");
            }
        }


        public virtual void Attack(IUnitView target, Action OnHit)
        {
            throw new NotImplementedException();
        }


        public virtual void TakeDamage(int damage)
        {
            throw new NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _eventListener?.OnClick(this);
        }
    }
}