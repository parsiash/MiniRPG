using MiniRPG.Logic.Metagame;
using MiniRPG.Navigation;
using System.Threading.Tasks;

namespace MiniRPG.Menu
{
    public class MenuPageBase : NavigationPageBase
    {
        protected IMetagameSimulation metagameSimulation { get; set; }
        protected INavigationLoader navigationLoader { get; set; }

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);
            
            //validate load data
            var loadData = data as MenuPageLoadData;
            if(loadData == null)
            {
                throw new NavigationException($"Loading menu page : {GetType().Name} failed. No load data is provided to {nameof(OnLoaded)} method.");
            }

            metagameSimulation = loadData.metagameSimulation;
            navigationLoader = loadData.navigationLoader;
            return true;
        }

    }
}