using MiniRPG.Metagame;
using MiniRPG.Navigation;

namespace MiniRPG.Menu
{
    public class MenuPageLoadData : INavigationData
    {
        public IMetagameSimulation metagameSimulation { get; set; }
        public INavigationLoader navigationLoader { get; set; }

        public MenuPageLoadData(IMetagameSimulation metagameSimulation, INavigationLoader navigationLoader)
        {
            this.metagameSimulation = metagameSimulation;
            this.navigationLoader = navigationLoader;
        }
    }
}