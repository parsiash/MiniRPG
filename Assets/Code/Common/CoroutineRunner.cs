using System;
using System.Collections;
using UnityEngine;

namespace MiniRPG.Common
{
    public class CoroutineRunner
    {
        protected float _currentValue;
        public float CurrentValue => _currentValue;

        protected float _startValue;
        protected float _targetValue;
        protected float _time;
        protected AnimationCurve _esasingCurve;
        
        protected Action<float> _onValueChanged;
        protected Action _onFinished;

        protected MonoBehaviour _behaviour;
        protected Coroutine _coroutine;

        public enum State
        {
            Initial,
            Started,
            Finished,
            Cancelled
        }

        public State state { get; protected set; }

        public CoroutineRunner(float startValue, float targetValue, float time, AnimationCurve esasingCurve, Action<float> onValueChanged, Action onFinished, MonoBehaviour behaviour)
        {
            _currentValue = _startValue;

            _startValue = startValue;
            _targetValue = targetValue;
            _time = time;

            _esasingCurve = esasingCurve;
            _onValueChanged = onValueChanged;
            _onFinished = onFinished;
            _behaviour = behaviour;

            state = State.Initial;
        }

        public void Start()
        {
            if(state == State.Initial)
            {
                _behaviour.StartCoroutine(Lerping());
                state = State.Initial;
            }
        }

        public void Stop()
        {
            if(state == State.Started)
            {
                _behaviour?.StopCoroutine(_coroutine);
                state = State.Cancelled;
            }
        }

        protected IEnumerator Lerping()
        {
            float currentTime = 0f;

            _onValueChanged(_currentValue);
            
            while(currentTime < _time)
            {
                var lerpTime = currentTime / _time;
                if(_esasingCurve != null)
                {
                    lerpTime = _esasingCurve.Evaluate(lerpTime);
                }

                _currentValue = Mathf.Lerp(_startValue, _targetValue, lerpTime);

                if(_onValueChanged != null)
                {
                    _onValueChanged(_currentValue);
                }

                yield return null;
                currentTime += Time.deltaTime;
            }

            state = State.Finished;

            if(_onFinished != null)
            {
                _onFinished();
            }
        }
    }
}