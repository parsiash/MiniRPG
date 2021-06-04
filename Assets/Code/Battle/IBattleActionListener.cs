using MiniRPG.BattleView;

namespace MiniRPG.Battle
{
    public interface IBattleActionListener
    {
        void OnAttack(int playerIndex, int attackerId, int targetId);
        void OnRandomAttack(int playerIndex, int attackerId);
        void OnUnitViewClick(IUnitView unitView);
        void OnUnitViewHold(IUnitView unitView);
    }
}                                                                                                                                      