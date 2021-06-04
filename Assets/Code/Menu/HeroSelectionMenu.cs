using MiniRPG.BattleLogic;
using MiniRPG.Common;
using MiniRPG.Metagame;
using MiniRPG.Navigation;
using MiniRPG.UI;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MiniRPG.Menu
{
    public class HeroSelectionMenu : MenuPageBase
    {
        public class LoadData : MenuLoadData
        {
            public IHeroAnouncementHandler heroAnouncementHandler { get; set; }
            public IOnScreenMessageFactory onScreenMessageFactory { get; set; }
            public HeroInfoPopup heroInfoPopup { get; set; }

            public LoadData(
                IMetagameSimulation metagameSimulation, 
                IHeroAnouncementHandler heroAnouncementHandler,
                IOnScreenMessageFactory onScreenMessageFactory,
                HeroInfoPopup heroInfoPopup) : base(metagameSimulation)
            {
                this.heroAnouncementHandler = heroAnouncementHandler;
                this.onScreenMessageFactory = onScreenMessageFactory;
                this.heroInfoPopup = heroInfoPopup;
            }
        }

        private HeroListPanel heroListPanel => RetrieveCachedComponentInChildren<HeroListPanel>();
        private IHeroAnouncementHandler _heroAnouncementHandler;
        private IOnScreenMessageFactory _onScreenMessageFactory;
        private HeroInfoPopup _heroInfoPopup;

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);

            var loadData = data as LoadData;
            if (loadData == null)
            {
                throw new NavigationException($"Loading Hero Selection Page failed. No load data is provided to {nameof(OnLoaded)} method.");
            }

            InitializeHeroListPanel();

            _heroInfoPopup = loadData.heroInfoPopup;

            //show hero anouncements
            _onScreenMessageFactory = loadData.onScreenMessageFactory;
            _heroAnouncementHandler = loadData.heroAnouncementHandler;

            return true;
        }

        protected void Update()
        {
            CheckAndShowHeroAnouncements();
        }

        private void CheckAndShowHeroAnouncements()
        {
            var heroAnouncements = _heroAnouncementHandler.FlushAnouncements();
            foreach (var heroAnouncement in heroAnouncements)
            {
                var heroButton = heroListPanel.GetHeroButton(heroAnouncement.heroId);
                if (heroButton)
                {
                    _onScreenMessageFactory.ShowMessage(new OnScreenMessage.Configuration(
                        heroAnouncement.text,
                        Color.green,
                        heroButton.GetWorldPosition(),
                        2f,
                        0.5f,
                        0.1f
                    ));
                }

                logger.LogDebug("Hero Anouncement" + heroAnouncement.heroId + " " + heroAnouncement.text);
            }
        }

        private void InitializeHeroListPanel()
        {
            //initialize UI based on playe profile
            var profile = metagameSimulation.User.Profile;
            heroListPanel.InitHeroes(
                profile.heroes.Select(
                    (hero) => new HeroButtonConfiguration(
                        profile.deck.ContainsHero(hero.heroId),
                        hero,
                        OnHeroButtonClick,
                        OnHeroButtonHold
                    )
            ));
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
                    _onScreenMessageFactory.ShowWarning("Deck is full");
                }else
                {
                    profile.deck.AddHero(heroId);
                    heroButton.Selected = true;
                }
            }
        }

        public void OnHeroButtonHold(HeroButton heroButton)
        {
            _heroInfoPopup.ShowPopup(
                HeroInfo.CreateFromHero(heroButton.Hero),
                heroButton.rectTranform.GetWorldPosition()
            );
        }

        public void OnClearDataButtonClick()
        {
            GameManager.Instance.ClearAndReset();
        }

        public async void OnStartBattleButtonClick()
        {
            var profile = metagameSimulation.User.Profile;
            var deck = profile.deck;
            if (deck.HeroCount != 3)
            {
                _onScreenMessageFactory.ShowWarning("Select 3 heroes");
                return;
            }

            await parentNavigator.ShowPage<Battle.BattlePage>(
                new Battle.BattlePage.LoadData(
                    new BattleLogic.BattleInitData(
                        new PlayerInitData(
                            0,
                            deck.heroIds.Select(hid => profile.GetHero(hid)).Select(
                                hero => ConvertToUnitInitData(hero)
                            ).ToArray()
                        ),
                        new PlayerInitData(
                                1,
                                new UnitInitData[] { ConvertToUnitInitData(GameManager.Instance.GenerateHero(-1)) }
                        )
                    ),
                    OnBattleResult,
                    _heroInfoPopup,
                    _onScreenMessageFactory
                )
            );
        }

        private async void OnBattleResult(BattleResult battleResult)
        {
            metagameSimulation.OnBattleResult(battleResult);

            //@TODO; this is a hack, everything should go in a battle loader component
            await GameManager.Instance.rootNavigator.ShowPage<Menu.HeroSelectionMenu>(
                new LoadData(metagameSimulation, _heroAnouncementHandler, _onScreenMessageFactory, _heroInfoPopup)
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
                ),
                hero
            );
        }
    }
}