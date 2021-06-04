using MiniRPG.Metagame;
using MiniRPG.UI;

namespace MiniRPG.Menu
{
    public class HeroSelectionMenuLoadData : MenuPageLoadData
    {
        public IHeroAnouncementHandler heroAnouncementHandler { get; set; }
        public IOnScreenMessageFactory onScreenMessageFactory { get; set; }
        public HeroInfoPopup heroInfoPopup { get; set; }

        public HeroSelectionMenuLoadData(
            IMetagameSimulation metagameSimulation, 
            IMenuLoader menuLoader,
            IHeroAnouncementHandler heroAnouncementHandler,
            IOnScreenMessageFactory onScreenMessageFactory,
            HeroInfoPopup heroInfoPopup) : base(metagameSimulation, menuLoader)
        {
            this.heroAnouncementHandler = heroAnouncementHandler;
            this.onScreenMessageFactory = onScreenMessageFactory;
            this.heroInfoPopup = heroInfoPopup;
        }
    }
}