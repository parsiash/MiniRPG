using MiniRPG.Logic.Battle;
using UnityEngine;

namespace MiniRPG.BattleView
{
    /// <summary>
    /// This is the interface of the visual representation of entities in battle view.
    /// </summary>
    public interface IEntityView
    {
        void Init(Entity entity, IEntityViewEventListener eventListener);
        Entity Entity { get; }
        Vector2 Position { get; set; }
    }
}