using MiniRPG.Common;
using MiniRPG.Metagame;

namespace MiniRPG
{
    public class Game
    {
        public IMetagameSimulation _metagameSimulation;
        public Common.ILogger _logger;

        public Game(IMetagameSimulation metagameSimulation, ILogger logger)
        {
            _metagameSimulation = metagameSimulation;
            _logger = logger;
        }
    }
}
