using System;
using MiniRPG.BattleLogic;
using MiniRPG.Common;
using UnityEngine;

namespace MiniRPG.BattleView
{
    public interface IUnitView : IEntityView
    {
        int Health { get; set; }
        void TakeDamage(int damage);
        void Attack(IUnitView target, System.Action OnHit);
    }

    public class UnitView : CommonBehaviour, IUnitView
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

        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private Unit _unit;
        public Entity Entity => _unit;

        public void Init(Entity entity)
        {
            if(entity is Unit)
            {
                _unit = entity as Unit;
                _health = _unit.healthComponent.health;
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
    }
}