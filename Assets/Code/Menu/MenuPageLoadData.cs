using MiniRPG.Metagame;
using MiniRPG.Navigation;

namespace MiniRPG.Menu
{
    public class MenuPageLoadData : INavigationData
    {
        public IMetagameSimulation metagameSimulation { get; set; }
        public IMenuLoader menuLoader { get; set; }

        public MenuPageLoadData(IMetagameSimulation metagameSimulation, IMenuLoader menuLoader)
        {
            this.metagameSimulation = metagameSimulation;
            this.menuLoader = menuLoader;
        }
    }
}