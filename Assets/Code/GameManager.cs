using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniRPG.BattleLogic;
using MiniRPG.Common;
using MiniRPG.Menu;
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

        public INavigator rootNavigator { get; private set; }
        private IPlayerDataRepository playerDataRepository;

        [SerializeField] private OnScreenMessage onScreenMessagePrefab;

        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            logger.Log($"Scene {scene.name} Loaded");

            //initialize root navigator
            rootNavigator = new Navigator(logger);
            rootNavigator.AddPage(FindPage<Menu.HeroSelectionMenu>());
            rootNavigator.AddPage(FindPage<Battle.BattlePage>());

            IHeroDataSource heroDataSource = new HeroDataSource(HeroTemplatesAsset.Instance, logger);

            //create the game object
            playerDataRepository = new PlayerDataRepository(LocalObjectStorage.Instance, logger);
            var profile = playerDataRepository.LoadUserProfile();
            if(profile == null)
            {
                profile = new UserProfile(
                    "GuestUser",
                    Enumerable.Range(0, 3).Select(i => heroDataSource.GetRandomHero(i)).ToArray(),
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


            IMetagameSimulation metagameSimulation = 
                new MetagameSimulation(
                    new User(
                        profile
                    ),
                    profileController,
                    heroDataSource,
                    logger
                );
            
            var rootMenuLoader = new RootMenuLoader(
                rootNavigator,
                metagameSimulation,
                heroAnouncementHandler,
                onScreenMessageFactory,
                heroInfoPopup
            );

            await rootMenuLoader.LoadHeroSelectionMenu();
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

        public void ClearAndReset()
        {
            playerDataRepository.ClearData();
            ResetGame();
        }

        private static void ResetGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
