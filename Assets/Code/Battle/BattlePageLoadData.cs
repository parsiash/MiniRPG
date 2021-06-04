using System;
using MiniRPG.BattleLogic;
using MiniRPG.BattleView;
using MiniRPG.Navigation;
using MiniRPG.UI;

namespace MiniRPG.Battle
{
    public class BattlePageLoadData : INavigationData
    {
        public BattleInitData battleInitData { get; private set ;}
        public IUnitViewFactory unitViewFactory { get; private set; }
        public Action<BattleResult> onBattleResultCallback { get; private set; }
        public HeroInfoPopup heroInfoPoup { get; private set; }
        public IOnScreenMessageFactory onScreenMessageFactory { get; private set; }

        public BattlePageLoadData(BattleInitData battleInitData, IUnitViewFactory unitViewFactory, Action<BattleResult> onBattleResultCallback, HeroInfoPopup heroInfoPoup, IOnScreenMessageFactory onScreenMessageFactory)
        {
            this.battleInitData = battleInitData;
            this.unitViewFactory = unitViewFactory;
            this.onBattleResultCallback = onBattleResultCallback;
            this.heroInfoPoup = heroInfoPoup;
            this.onScreenMessageFactory = onScreenMessageFactory;
        }
    }
}