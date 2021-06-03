using MiniRPG.Common;
using MiniRPG.Metagame;
using MiniRPG.Navigation;
using System.Linq;
using System.Threading.Tasks;

namespace MiniRPG.Menu
{
    public class UIComponent : CommonBehaviour
    {

    }

    public class MenuPageBase : NavigationPageBase
    {
        public class LoadData : INavigationData
        {
            public IMetagameSimulation metagameSimulation { get; set; }

            public LoadData(IMetagameSimulation metagameSimulation)
            {
                this.metagameSimulation = metagameSimulation;
            }
        }

        protected IMetagameSimulation metagameSimulation { get; set; }

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);
            
            //validate load data
            var loadData = data as LoadData;
            if(loadData == null)
            {
                throw new NavigationException($"Loading Hero Selection Page failed. No load data is provided to {nameof(OnLoaded)} method.");
            }

            metagameSimulation = loadData.metagameSimulation;
            return true;
        }

    }

    public class HeroSelectionMenu : MenuPageBase
    {
   
        private HeroListPanel heroListPanel => RetrieveCachedComponentInChildren<HeroListPanel>();

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);

            //initialize UI based on playe profile
            heroListPanel.InitHeroes(
                metagameSimulation.User.Profile.heroes.Select(
                    (hero) => new HeroButtonConfiguration(
                        false,
                        hero,
                        OnHeroButtonClick
                    )
            ));

            return true;
        }

        public void OnHeroButtonClick(HeroButton heroButton)
        {
            heroButton.Selected = !heroButton.Selected;
        }
    }
}