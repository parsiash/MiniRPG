using MiniRPG.Metagame;
using MiniRPG.Navigation;
using System.Threading.Tasks;

namespace MiniRPG.Menu
{
    public class HeroSelectionMenu : NavigationPageBase
    {
        public class LoadData : INavigationData
        {
            public MetagameSimulation metagameSimulation { get; set; }

            public LoadData(MetagameSimulation metagameSimulation)
            {
                this.metagameSimulation = metagameSimulation;
            }
        }

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);

            //validate load data
            var loadData = data as LoadData;
            if(loadData == null)
            {
                throw new NavigationException($"Loading Hero Selection Page failed. No load data is provided to {nameof(OnLoaded)} method.");
            }

            //initialize UI based on playe profile

            return true;
        }
    }
}