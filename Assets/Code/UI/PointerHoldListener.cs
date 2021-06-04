using System;
using MiniRPG.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MiniRPG.UI
{
    public class PointerHoldListener : CommonBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float minHoldSeconds = 0.4f;
        [SerializeField] private UnityEvent onHold;

        public UnityEvent OnHold => onHold;
        
        private bool _isHoldingDown;
        private float _currentHoldTime;

        void Update()
        {
            if(_isHoldingDown)
            {
                _currentHoldTime += Time.deltaTime;

                 if(_currentHoldTime >= minHoldSeconds)
                {
                    _isHoldingDown = false;
                    onHold?.Invoke();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isHoldingDown = true;
            _currentHoldTime = 0f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isHoldingDown = false;
        }
    }
}