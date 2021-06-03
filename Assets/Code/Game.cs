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
        public Common.ILogger _logger;

        public Game(IMetagameSimulation metagameSimulation, IHeroAnouncementHandler heroAnouncementHandler, IOnScreenMessageFactory onScreenMessageFactory, ILogger logger)
        {
            this.metagameSimulation = metagameSimulation;
            this.heroAnouncementHandler = heroAnouncementHandler;
            this.onScreenMessageFactory = onScreenMessageFactory;
            _logger = logger;
        }
    }
}
