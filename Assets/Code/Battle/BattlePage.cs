using System.Threading.Tasks;
using MiniRPG.BattleLogic;
using MiniRPG.Common;
using MiniRPG.Navigation;
using TMPro;

namespace MiniRPG.Battle
{
    public class BattlePage : NavigationPageBase
    {
        public class OnLoadData : INavigationData
        {
            public BattleInitData battleInitData { get; set ;}

            public OnLoadData(BattleInitData battleInitData)
            {
                this.battleInitData = battleInitData;
            }
        }

        public BattleController battleController => RetrieveCachedComponentInChildren<BattleController>();
        public BattleResultPage battleResultPage => RetrieveCachedComponentInChildren<BattleResultPage>();

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);

            var loadData = data as OnLoadData;
            if(loadData == null)
            {
                throw new NavigationException($"Loading battle page failed. No load data is provided to {nameof(OnLoaded)} method.");
            }

            battleResultPage.Hide();

            //init battle controller
            battleController.Init(new BattleControllerConfiguration(
                loadData.battleInitData,
                OnBattleFinish
            ));

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
                OnBattlseResultPageFinish
            );
        }

        private async void OnBattlseResultPageFinish()
        {
            await parentNavigator.ShowPage<Menu.HeroSelectionMenu>(new Menu.MenuPageBase.LoadData(GameManager.Instance.game.metagameSimulation));
        }
    }
}