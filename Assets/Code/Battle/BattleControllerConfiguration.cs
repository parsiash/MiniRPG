using System;
using MiniRPG.Logic.Battle;
using MiniRPG.BattleView;
using MiniRPG.UI;

namespace MiniRPG.Battle
{
    /// <summary>
    /// The configuration class used for initializing the BattleController.
    /// </summary>
    public class BattleControllerConfiguration
    {
        public BattleInitData battleInitData { get; private set; }
        public IUnitViewFactory unitViewFactory { get; private set; }
        public System.Action<BattleResult> OnBattleFinishCallback { get; private set; }
        public System.Action<IUnitView> OnUnitHoldCallback { get; private set; }
        public IOnScreenMessageFactory onScreenMessageFactory { get; private set; }

        public BattleControllerConfiguration(BattleInitData battleInitData, IUnitViewFactory unitViewFactory, Action<BattleResult> onBattleFinishCallback, Action<IUnitView> onUnitHoldCallback, IOnScreenMessageFactory onScreenMessageFactory)
        {
            this.battleInitData = battleInitData;
            this.unitViewFactory = unitViewFactory;
            OnBattleFinishCallback = onBattleFinishCallback;
            OnUnitHoldCallback = onUnitHoldCallback;
            this.onScreenMessageFactory = onScreenMessageFactory;
        }
    }
}                                                                                                                                      