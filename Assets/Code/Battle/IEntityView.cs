using MiniRPG.Logic.Battle;
using UnityEngine;

namespace MiniRPG.BattleView
{
    public interface IEntityView
    {
        void Init(Entity entity, IEntityViewEventListener eventListener);
        Entity Entity { get; }
        Vector2 Position { get; set; }
    }
}