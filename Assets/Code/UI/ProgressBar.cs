using System.Collections;
using DG.Tweening;
using MiniRPG.Common;
using UnityEngine;
using UnityEngine.UI;

namespace MiniRPG.UI
{
    public class ProgressBar : CommonBehaviour
    {
        protected float _currentValue;
        protected float _maxValue;

        public float Progress => (_maxValue == 0) ? 0f : _currentValue / _maxValue;

        [SerializeField] protected Image fillImage;

        [Header("Amount of time to fill a whole bar")]
        [SerializeField] protected float fillingTime = 1f;
        private Tweener _fillingTweener;

        public void Init(float maxValue, float initialValue)
        {
            _maxValue = maxValue;
            SetValue(initialValue);
        }

        protected virtual void SetValue(float value)
        {
            _currentValue = value;
            fillImage.fillAmount = Progress;
        }

        public void Subtract(float amount)
        {
            FillTo(_currentValue - amount);
        }

        public void FillTo(float targetValue)
        {
            StopFilling();

            _fillingTweener = DOTween.To(
                () => _currentValue,
                (v) => SetValue(v),
                targetValue,
                Mathf.Max((targetValue - _currentValue) / _maxValue * fillingTime, 0.5f)
            );
        }

        private void StopFilling()
        {
            if(_fillingTweener != null)
            {
                _fillingTweener.Kill();
                _fillingTweener = null;
            }
        }
    }
}