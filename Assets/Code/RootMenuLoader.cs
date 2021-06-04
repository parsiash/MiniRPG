using System.Threading.Tasks;
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

        public RootMenuLoader(INavigator rootNavigator, IMetagameSimulation metagameSimulation, IHeroAnouncementHandler heroAnouncementHandler, IOnScreenMessageFactory onScreenMessageFactory, HeroInfoPopup heroInfoPopup)
        {
            this.rootNavigator = rootNavigator;
            this.metagameSimulation = metagameSimulation;
            this.heroAnouncementHandler = heroAnouncementHandler;
            this.onScreenMessageFactory = onScreenMessageFactory;
            this.heroInfoPopup = heroInfoPopup;
        }

        public async Task<bool> LoadHeroSelectionMenu()
        {
            //@TODO; this is a hack, everything should go in a battle loader component
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
    }
}
