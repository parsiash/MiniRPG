using System;
using System.Threading.Tasks;
using MiniRPG.Logic.Battle;
using MiniRPG.Common;
using MiniRPG.Navigation;
using MiniRPG.UI;

namespace MiniRPG.Battle
{
    /// <summary>
    /// The navigation page of battle.
    /// </summary>
    public class BattlePage : NavigationPageBase
    {
        public BattleController battleController => RetrieveCachedComponentInChildren<BattleController>();
        public BattleResultPage battleResultPage => RetrieveCachedComponentInChildren<BattleResultPage>();
        private Action<BattleResult> _onBattleResultCallback;
        private HeroInfoPopup _heroInfoPopup;


        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);

            var loadData = data as BattlePageLoadData;
            if(loadData == null)
            {
                throw new NavigationException($"Loading battle page failed. No load data is provided to {nameof(OnLoaded)} method.");
            }

            battleResultPage.Hide();

            //init battle controller
            battleController.Init(new BattleControllerConfiguration(
                loadData.battleInitData,
                loadData.unitViewFactory,
                OnBattleFinish,
                (unitView) => _heroInfoPopup.ShowPopup(HeroInfo.CreateFromHero(unitView.Unit.hero), unitView.Position),
                loadData.onScreenMessageFactory
            ));

            _onBattleResultCallback = loadData.onBattleResultCallback;
            _heroInfoPopup = loadData.heroInfoPoup;

            battleController.StartBattle();

            return true;
        }

        public override async Task<bool> OnHide()
        {
            await base.OnHide();

            battleController.Clear();

            return true;
        }

        public void OnBattleFinish(BattleResult battleResult)
        {
            if(battleResult == null)
            {
                logger.LogError("Battle result is null.");
                return;
            }

            battleResultPage.ShowBattleResult(
                battleResult.winnerPlayerIndex == 0 ? BattleResultPage.BattleResultStatus.Win : BattleResultPage.BattleResultStatus.Lose,
                () => OnBattlseResultPageFinish(battleResult)
            );
        }

        private void OnBattlseResultPageFinish(BattleResult battleResult)
        {
            _onBattleResultCallback(battleResult);
        }
    }
}