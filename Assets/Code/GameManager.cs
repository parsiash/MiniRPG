using MiniRPG.Common;
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

        public IServiceCollection serviceCollection { get; private set; }

        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            logger.Log($"Scene {scene.name} Loaded");

            //configure services
            serviceCollection = new ServiceCollection(logger);
            IServiceInitializer serviceInitializer = new ServiceInitializer(serviceCollection, logger);
            serviceInitializer.ConfigureServices();

            //load hero selection menu
            var rootMenuLoader = serviceCollection.GetService<IMenuLoader>();
            await rootMenuLoader.LoadHeroSelectionMenu();
        }

        public void ClearAndReset()
        {
            var playerDataRepository = serviceCollection.GetService<IPlayerDataRepository>();
            if(playerDataRepository != null)
            {
                playerDataRepository.ClearData();
                ResetGame();
            }
        }

        private static void ResetGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
