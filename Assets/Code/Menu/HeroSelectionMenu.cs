using MiniRPG.BattleLogic;
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
            var profile = metagameSimulation.User.Profile;

            heroListPanel.InitHeroes(
                profile.heroes.Select(
                    (hero) => new HeroButtonConfiguration(
                        profile.deck.ContainsHero(hero.heroId),
                        hero,
                        OnHeroButtonClick
                    )
            ));

            return true;
        }

        public void OnHeroButtonClick(HeroButton heroButton)
        {
            var profile = metagameSimulation.User.Profile;
            var heroId = heroButton.Hero.heroId;

            if (profile.deck.ContainsHero(heroId))
            {
                profile.deck.RemoveHero(heroId);
                heroButton.Selected = false;
            }else
            {
                if(!profile.deck.HasCapacity)
                {
                    logger.LogError("Cannot add hero to deck: Deck is full.");
                }else
                {
                    profile.deck.AddHero(heroId);
                    heroButton.Selected = true;
                }
            }
        }

        public async void OnStartBattleButtonClick()
        {
            var profile = metagameSimulation.User.Profile;
            var deck = profile.deck;
            if (deck.IsEmpty)
            {
                logger.LogError("Cannot Start the battle. Deck is empty");
                return;
            }

            await parentNavigator.ShowPage<Battle.BattlePage>(
                new Battle.BattlePage.OnLoadData(
                    new BattleLogic.BattleInitData(
                        new PlayerInitData(
                            0,
                            deck.heroIds.Select(hid => profile.GetHero(hid)).Select(
                                hero => ConvertToUnitInitData(hero)
                            ).ToArray()
                        ),
                        new PlayerInitData(
                                1,
                                new UnitInitData[] { ConvertToUnitInitData(GameManager.Instance.GenerateHero(100)) }
                        )
                    )
                )
            );
        }

        private static UnitInitData ConvertToUnitInitData(ProfileHero hero)
        {
            return new UnitInitData(
                                                    hero.name,
                                                    hero.level,
                                                    hero.experience,
                                                    new UnitStat(
                                                        hero.attack,
                                                        hero.health
                                                    )
                                                );
        }
    }
}