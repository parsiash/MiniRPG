using System.Threading.Tasks;
using MiniRPG.Battle;
using MiniRPG.BattleLogic;
using MiniRPG.BattleView;
using MiniRPG.Menu;
using MiniRPG.Metagame;
using MiniRPG.Navigation;
using MiniRPG.UI;

namespace MiniRPG
{
    public class RootMenuLoader : IMenuLoader
    {
        private INavigator rootNavigator { get; set; }
        private IMetagameSimulation metagameSimulation { get; set; }
        private IHeroAnouncementHandler heroAnouncementHandler { get; set; }
        private IOnScreenMessageFactory onScreenMessageFactory { get; set; }
        private HeroInfoPopup heroInfoPopup { get; set; }
        private IUnitViewFactory unitViewFactory { get; set; }

        public RootMenuLoader(INavigator rootNavigator, IMetagameSimulation metagameSimulation, IHeroAnouncementHandler heroAnouncementHandler, IOnScreenMessageFactory onScreenMessageFactory, HeroInfoPopup heroInfoPopup, IUnitViewFactory unitViewFactory)
        {
            this.rootNavigator = rootNavigator;
            this.metagameSimulation = metagameSimulation;
            this.heroAnouncementHandler = heroAnouncementHandler;
            this.onScreenMessageFactory = onScreenMessageFactory;
            this.heroInfoPopup = heroInfoPopup;
            this.unitViewFactory = unitViewFactory;
        }

        public async Task<bool> LoadHeroSelectionMenu()
        {
            return await rootNavigator.ShowPage<Menu.HeroSelectionMenu>(
                new HeroSelectionMenuLoadData(
                    metagameSimulation, 
                    this,
                    heroAnouncementHandler, 
                    onScreenMessageFactory, 
                    heroInfoPopup
                )
            );
        }

        public async Task<bool> StartBattle()
        {
            return await rootNavigator.ShowPage<Battle.BattlePage>(
                new BattlePageLoadData(
                    metagameSimulation.StartBattle(),
                    unitViewFactory,
                    OnBattleResult,
                    heroInfoPopup,
                    onScreenMessageFactory
                )
            );   
        }

        private async void OnBattleResult(BattleResult battleResult)
        {
            metagameSimulation.OnBattleResult(battleResult);
            await LoadHeroSelectionMenu();
        }
    }
}
