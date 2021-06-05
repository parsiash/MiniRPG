using System;
using MiniRPG.Logic.Battle;
using MiniRPG.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace MiniRPG.BattleView
{
    public interface IEntityViewEventListener
    {
        void OnClick(IEntityView entityView);
        void OnHold(IEntityView entityView);
        void OnEntityDestroy(IEntityView entityView);
    }

    public interface IUnitView : IEntityView
    {
        Unit Unit { get; }
        int Health { get; set; }
        void TakeDamage(int damage);
        void Attack(IUnitView target, System.Action OnHit, System.Action OnFinish);
        void SetSortingOrder(int sortingOrder);
        void SetInfoBarVisible(bool visible);
    }

    public class UnitView : PoolableBehaviour, IUnitView, IPointerClickHandler
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

        private UnitViewInfoUI infoUI => RetrieveCachedComponentInChildren<UnitViewInfoUI>();
        private SpriteRenderer heroSprite => RetrieveCachedComponentInChildren<SpriteRenderer>();

        private IUnitViewAnimationController animationController
        {
            get
            {
                if(_animationController == null)
                {
                    _animationController = GetComponentInChildren<IUnitViewAnimationController>();
                }

                return _animationController;
            }
        }
        private IUnitViewAnimationController _animationController;

        private SortingGroup sortingGroup
        {
            get
            {
                if(!_sortingGroup)
                {
                    _sortingGroup = gameObject.AddComponent<SortingGroup>();
                }

                return _sortingGroup;
            }
        }
        private SortingGroup _sortingGroup;


        public void Init(Entity entity, IEntityViewEventListener eventListener)
        {
            if(entity is Unit)
            {
                _unit = entity as Unit;
                _health = _unit.healthComponent.health;
                
                _eventListener = eventListener;

                infoUI.Init(_unit.healthComponent.health, _unit.healthComponent.maxHealth);
                animationController.Init(this);

                heroSprite.color = _unit.hero.color.GetUnityColor();

                transform.localScale = Vector3.one * _unit.hero.size;
                
            }else
            {
                logger.LogError($"Cannot init unit view : {gameObject.name}. The given entity is not a unit entity.");
            }
        }

        public virtual void Attack(IUnitView target, Action OnHit, Action OnFinish)
        {
            animationController.PlayAttack(target.Position, OnHit, OnFinish);
        }

        public virtual void TakeDamage(int damage)
        {
            infoUI.TakeDamage(
                damage,
                () => {
                    if(_unit.IsDead)
                    {
                        _eventListener.OnEntityDestroy(this);
                    }
                }
            );
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_unit.IsDead)
            {
                return;
            }
            
            _eventListener?.OnClick(this);
        }

        public void OnHold()
        {
            _eventListener?.OnHold(this);
        }

      
        public void SetSortingOrder(int sortingOrder)
        {
            sortingGroup.sortingOrder = sortingOrder;
        }

        public void SetInfoBarVisible(bool visible)
        {
            infoUI.SetVisible(visible);
        }
    }
}