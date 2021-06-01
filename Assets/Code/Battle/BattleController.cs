using MiniRPG.BattleLogic;
using MiniRPG.BattleView;

namespace MiniRPG.Battle
{
    /// <summary>
    /// The class responsible for managing the battle simulation and syncing it with battle view.
    /// </summary>
    public class BattleController
    {
        private IBattleSimulation _battleSimulation;
        private IBattleView _battleView;

        public BattleController(IBattleSimulation battleSimulation, IBattleView battleView)
        {
            _battleSimulation = battleSimulation;
            _battleView = battleView;

            _battleView.Init(battleSimulation);
        }

        public void StartBattle()
        { 
            _battleSimulation.StartBattle();
        }
    }
}                                                                                                                                      