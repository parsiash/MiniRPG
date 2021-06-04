using System;
using UnityEngine;

namespace MiniRPG.BattleView
{
    /// <summary>
    /// The base interface for any component that handles UnitView animations.
    /// Using an interface here is for being able to have any implementation of these methods using any animation tools/libraries.
    /// </summary>
    public interface IUnitViewAnimationController
    {
        void Init(IUnitView unitView);
        void PlayAttack(Vector2 targetPosition, Action OnHit, Action OnFinish);
    }
}