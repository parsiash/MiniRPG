using MiniRPG.Logic.Battle;
using MiniRPG.Common;
using MiniRPG.Navigation;
using MiniRPG.UI;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MiniRPG.Menu
{
    /// <summary>
    /// The menu when players choose their heroes.
    /// </summary>
    public class HeroSelectionMenu : MenuPageBase
    {
        private HeroListPanel heroListPanel => RetrieveCachedComponentInChildren<HeroListPanel>();
        private IHeroAnouncementHandler _heroAnouncementHandler;
        private IOnScreenMessageFactory _onScreenMessageFactory;
        private HeroInfoPopup _heroInfoPopup;

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);

            var loadData = data as HeroSelectionMenuLoadData;
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

            var newDeck = profile.deck.GetCopy();

            if (profile.deck.ContainsHero(heroId))
            {
                newDeck.RemoveHero(heroId);
                metagameSimulation.OnDeckChange(newDeck);
                
                heroButton.Selected = false;
            }else
            {
                if(!profile.deck.HasCapacity)
                {
                    _onScreenMessageFactory.ShowWarning("Deck is full");
                }else
                {
                    newDeck.AddHero(heroId);
                    metagameSimulation.OnDeckChange(newDeck);

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

        public async void OnStartBattleButtonClick()
        {
            var profile = metagameSimulation.User.Profile;
            var deck = profile.deck;
            if (deck.HeroCount != 3)
            {
                _onScreenMessageFactory.ShowWarning("Select 3 heroes");
                return;
            }

            await navigationLoader.StartBattle();
        }
        
        public void OnClearDataButtonClick()
        {
            GameManager.Instance.ClearAndReset();
        }


        protected void Update()
        {
            //This check is for showing hero anouncements with on screen messages
            //It is not a desirable approach to check in update, The better approach is to 
            //Add OnAfterLoaded life-cycle method to INavigationPage and check it there.
            CheckAndShowHeroAnouncements();
        }

        /// <summary>
        /// Checks all the hero anouncements stored in HeroAnouncementHandler and shows them.
        /// </summary>
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
    }
}