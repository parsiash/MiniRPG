using MiniRPG.BattleLogic;
using MiniRPG.BattleView;
using MiniRPG.Common;

namespace MiniRPG.Battle
{
    public class BattleControllerInitData
    {
        public BattleInitData battleInitData { get; set; }

        public BattleControllerInitData(BattleInitData battleInitData)
        {
            this.battleInitData = battleInitData;
        }
    }

    /// <summary>
    /// The class responsible for managing the battle simulation and syncing it with battle view.
    /// </summary>
    public class BattleController : CommonBehaviour
    {
        private IBattleSimulation _battleSimulation;
        private IBattleView battleView => RetrieveCachedComponentInChildren<BattleView.BattleView>();

        public void Init(BattleControllerInitData initData)
        {
            _battleSimulation = new BattleSimulation(initData.battleInitData, logger);
        }

        public void StartBattle()
        { 
            _battleSimulation.StartBattle();
        }
    }
}                                                                                                                                      