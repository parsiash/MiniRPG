using MiniRPG.Logic.Metagame;
using MiniRPG.Navigation;

namespace MiniRPG.Menu
{
    /// <summary>
    /// The base class for Navigation Data of the Menu Pages.
    /// </summary>
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