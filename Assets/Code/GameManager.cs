using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniRPG.BattleLogic;
using MiniRPG.Common;
using MiniRPG.Metagame;
using MiniRPG.Navigation;
using MiniRPG.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniRPG
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        protected override void Init()
        {
            logger.Log("Game Manager Init");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

#if DEBUG
        protected void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
        }
#endif

        public Game game { get; private set; }
        public INavigator rootNavigator { get; private set; }

        [SerializeField] private OnScreenMessage onScreenMessagePrefab;

        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            logger.Log($"Scene {scene.name} Loaded");

            //initialize root navigator
            rootNavigator = new Navigator(logger);
            rootNavigator.AddPage(FindPage<Menu.HeroSelectionMenu>());
            rootNavigator.AddPage(FindPage<Battle.BattlePage>());

            //create the game object
            var playerDataRepository = new PlayerDataRepository(LocalObjectStorage.Instance, logger);
            var profile = playerDataRepository.LoadUserProfile();
            if(profile == null)
            {
                profile = new UserProfile(
                    "GuestUser",
                    Enumerable.Range(1, 3).Select(i => GenerateHero(i)).ToArray(),
                    new ProfileDeck(),
                    0
                );

                playerDataRepository.SaveUserProfile(profile);
            }

            var profileController = new ProfileController(profile, playerDataRepository, logger);
            
            var heroAnouncementHandler = new HeroAnouncementHandler(logger);
            profileController.AddListener(heroAnouncementHandler);

            var onScreenMessageFactory = new OnScreenMessageFactory(onScreenMessagePrefab);

            var heroInfoPopup = Object.FindObjectOfType(typeof(HeroInfoPopup), true) as HeroInfoPopup;


            game = new Game(
                new MetagameSimulation(
                    new User(
                        profile
                    ),
                    profileController,
                    logger
                ),
                heroAnouncementHandler,
                onScreenMessageFactory,
                heroInfoPopup,
                playerDataRepository,
                logger
            );
            

            //show hero selection menu
            await rootNavigator.ShowPage<Menu.HeroSelectionMenu>(
                new Menu.HeroSelectionMenu.LoadData(
                    game.metagameSimulation,
                    game.heroAnouncementHandler,
                    game.onScreenMessageFactory,
                    game.heroInfoPopup
                )
            );
        }

         private T FindPage<T>() where T : NavigationPageBase
        {
            var page = Object.FindObjectOfType<T>(true);
            if(!page)
            {
                logger.LogError($"Cannot start the game. No Page of type {typeof(T).Name} Found.");
                return null;
            }

            return page;
        }

        public ProfileHero GenerateHero(int heroId)
        {
            return new ProfileHero(
                heroId,
                "Hero_" + heroId,
                Random.Range(1, 20),
                Random.Range(1, 20),
                Random.Range(10, 15),
                Random.Range(20, 30)
            );
        }

        public void ClearAndReset()
        {
            game.playerDataRepository.ClearData();
            ResetGame();
        }

        private static void ResetGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
