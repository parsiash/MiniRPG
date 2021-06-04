using System;
using DG.Tweening;
using MiniRPG.Common;
using UnityEngine;

namespace MiniRPG.BattleView
{
    /// <summary>
    /// A UnitView animation controller implemented using DoTween.
    /// </summary>
    public class UnitViewTweenAnimationController : CommonBehaviour, IUnitViewAnimationController
    {
        private IUnitView _unitView;

        public void Init(IUnitView unitView)
        {
            _unitView = unitView;
        }

        public void PlayAttack(Vector2 targetPosition, Action OnHit, Action OnFinish)
        {
            var initialPosition = _unitView.Position;

            //move to target
            var movingSequence = DOTween.Sequence();

            movingSequence.Append(
                DOTween.To(
                    () => _unitView.Position,
                    (pos) => _unitView.Position = pos,
                    targetPosition,
                    0.6f
                )
            );

            //call on hit
            if(OnHit != null)
            {
                movingSequence.AppendCallback(() => OnHit());
            }

            movingSequence.Append(
                DOTween.To(
                    () => _unitView.Position,
                    (pos) => _unitView.Position = pos,
                    initialPosition,
                    0.3f
                )
            );

            if(OnFinish != null)
            {
                movingSequence.AppendCallback(() => OnFinish());
            }
        }
    }
}