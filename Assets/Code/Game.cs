using MiniRPG.Common;
using MiniRPG.Metagame;

namespace MiniRPG
{
    public class Game
    {
        public IMetagameSimulation  metagameSimulation { get; private set; }
        public Common.ILogger _logger;

        public Game(IMetagameSimulation metagameSimulation, ILogger logger)
        {
            this.metagameSimulation = metagameSimulation;
            _logger = logger;
        }
    }
}
