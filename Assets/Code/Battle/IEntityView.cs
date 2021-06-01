using MiniRPG.BattleLogic;
using UnityEngine;

namespace MiniRPG.BattleView
{
    public interface IEntityView
    {
        void Init(Entity entity);
        Entity Entity { get; }
        Vector2 Position { get; set; }
    }
}