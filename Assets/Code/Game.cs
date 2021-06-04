using MiniRPG.Common;
using MiniRPG.Metagame;
using MiniRPG.UI;

namespace MiniRPG
{
    public class Game
    {
        public IMetagameSimulation  metagameSimulation { get; private set; }
        public IHeroAnouncementHandler heroAnouncementHandler { get; private set; }
        public IOnScreenMessageFactory onScreenMessageFactory { get; private set; }
        public HeroInfoPopup heroInfoPopup { get; private set; }
        public Common.ILogger _logger;

        public Game(IMetagameSimulation metagameSimulation, IHeroAnouncementHandler heroAnouncementHandler, IOnScreenMessageFactory onScreenMessageFactory, HeroInfoPopup heroInfoPopup, ILogger logger)
        {
            this.metagameSimulation = metagameSimulation;
            this.heroAnouncementHandler = heroAnouncementHandler;
            this.onScreenMessageFactory = onScreenMessageFactory;
            this.heroInfoPopup = heroInfoPopup;
            _logger = logger;
        }
    }
}
