using MiniRPG.Metagame;
using MiniRPG.Navigation;
using System.Threading.Tasks;

namespace MiniRPG.Menu
{
    public class MenuPageBase : NavigationPageBase
    {
        public class MenuLoadData : INavigationData
        {
            public IMetagameSimulation metagameSimulation { get; set; }

            public MenuLoadData(IMetagameSimulation metagameSimulation)
            {
                this.metagameSimulation = metagameSimulation;
            }
        }

        protected IMetagameSimulation metagameSimulation { get; set; }

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);
            
            //validate load data
            var loadData = data as MenuLoadData;
            if(loadData == null)
            {
                throw new NavigationException($"Loading menu page : {GetType().Name} failed. No load data is provided to {nameof(OnLoaded)} method.");
            }

            metagameSimulation = loadData.metagameSimulation;
            return true;
        }

    }
}