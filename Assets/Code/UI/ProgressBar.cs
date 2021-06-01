using System.Collections;
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
        [SerializeField] protected AnimationCurve fillingCurve;

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


        public void FillTo(float targetValue)
        {
            StopFilling();

            _coroutineRunner = new CoroutineRunner(
                _currentValue,
                targetValue,
                Mathf.Max((targetValue - _currentValue) / _maxValue * fillingTime, 0.5f),
                fillingCurve,
                (v) => SetValue(v),
                () => SetValue(targetValue),
                this
            );

            _coroutineRunner.Start();
        }

        private CoroutineRunner _coroutineRunner;

        private void StopFilling()
        {
            if(_coroutineRunner != null)
            {
                _coroutineRunner.Stop();
                _coroutineRunner = null;
            }
        }
    }
}