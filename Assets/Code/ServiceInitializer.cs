using System.Linq;
using MiniRPG.Common;
using MiniRPG.Metagame;
using MiniRPG.Navigation;
using MiniRPG.UI;
using UnityEngine;

namespace MiniRPG
{
    public interface IServiceInitializer
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }

    public class ServiceInitializer : IServiceInitializer
    {
        private Common.ILogger _logger;

        public ServiceInitializer(Common.ILogger logger)
        {
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            INavigator rootNavigator = ConfigureRootNavigator(serviceCollection);

            IHeroDataSource heroDataSource = ConfigureHeroDataSource(serviceCollection);
            IPlayerDataRepository playerDataRepository = ConfigurePlayerDataRepository(serviceCollection, heroDataSource);
            IProfileController profileController = ConfigureProfileController(serviceCollection, playerDataRepository);
            
            IHeroAnouncementHandler heroAnouncementHandler = ConfigureHeroAnouncementHandler(profileController);
            IOnScreenMessageFactory onScreenMessageFactory = ConfigureOnScreenMessageFactory(serviceCollection);
            HeroInfoPopup heroInfoPopup = ConfigureHeroInfoPopup(serviceCollection);

            IMetagameSimulation metagameSimulation = ConfigureMetagameSimulation(serviceCollection, heroDataSource, profileController);
            IMenuLoader rootMenuLoader = ConfigureRootMenuLoader(serviceCollection, rootNavigator, heroAnouncementHandler, onScreenMessageFactory, heroInfoPopup, metagameSimulation);
        }

        private static IMenuLoader ConfigureRootMenuLoader(IServiceCollection serviceCollection, INavigator rootNavigator, IHeroAnouncementHandler heroAnouncementHandler, IOnScreenMessageFactory onScreenMessageFactory, HeroInfoPopup heroInfoPopup, IMetagameSimulation metagameSimulation)
        {
            IMenuLoader rootMenuLoader = new RootMenuLoader(
                            rootNavigator,
                            metagameSimulation,
                            heroAnouncementHandler,
                            onScreenMessageFactory,
                            heroInfoPopup
                        );
            serviceCollection.AddService<IMenuLoader>(rootMenuLoader);
            return rootMenuLoader;
        }

        private IMetagameSimulation ConfigureMetagameSimulation(IServiceCollection serviceCollection, IHeroDataSource heroDataSource, IProfileController profileController)
        {
            IMetagameSimulation metagameSimulation =
                            new MetagameSimulation(
                                new User(
                                    profileController.Profile
                                ),
                                profileController,
                                heroDataSource,
                                _logger
                            );
            serviceCollection.AddService<IMetagameSimulation>(metagameSimulation);
            return metagameSimulation;
        }

        private static HeroInfoPopup ConfigureHeroInfoPopup(IServiceCollection serviceCollection)
        {
            var heroInfoPopup = Object.FindObjectOfType(typeof(HeroInfoPopup), true) as HeroInfoPopup;
            serviceCollection.AddService<HeroInfoPopup>(heroInfoPopup);
            return heroInfoPopup;
        }

        private IOnScreenMessageFactory ConfigureOnScreenMessageFactory(IServiceCollection serviceCollection)
        {
            var onScreenMessagePrefab = Resources.Load<OnScreenMessage>("OnScreenMessage");
            IOnScreenMessageFactory onScreenMessageFactory = new OnScreenMessageFactory(onScreenMessagePrefab);
            serviceCollection.AddService<IOnScreenMessageFactory>(onScreenMessageFactory);
            return onScreenMessageFactory;
        }

        private IHeroAnouncementHandler ConfigureHeroAnouncementHandler(IProfileController profileController)
        {
            var heroAnouncementHandler = new HeroAnouncementHandler(_logger);
            profileController.AddListener(heroAnouncementHandler);
            return heroAnouncementHandler;
        }

        private IProfileController ConfigureProfileController(IServiceCollection serviceCollection, IPlayerDataRepository playerDataRepository)
        {
            var profileController = new ProfileController(playerDataRepository.LoadUserProfile(), playerDataRepository, _logger);
            serviceCollection.AddService<IProfileController>(profileController);
            return profileController;
        }

        private IPlayerDataRepository ConfigurePlayerDataRepository(IServiceCollection serviceCollection, IHeroDataSource heroDataSource)
        {
            IPlayerDataRepository playerDataRepository = new PlayerDataRepository(LocalObjectStorage.Instance, _logger);

            var profile = playerDataRepository.LoadUserProfile();
            if (profile == null)
            {
                profile = new UserProfile(
                    "GuestUser",
                    Enumerable.Range(0, 3).Select(i => heroDataSource.GetRandomHero(i)).ToArray(),
                    new ProfileDeck(),
                    0
                );

                playerDataRepository.SaveUserProfile(profile);
            }

            serviceCollection.AddService<IPlayerDataRepository>(playerDataRepository);
            return playerDataRepository;
        }

        private IHeroDataSource ConfigureHeroDataSource(IServiceCollection serviceCollection)
        {
            IHeroDataSource heroDataSource = new HeroDataSource(HeroTemplatesAsset.Instance, _logger);
            serviceCollection.AddService<IHeroDataSource>(heroDataSource);
            return heroDataSource;
        }

        private INavigator ConfigureRootNavigator(IServiceCollection serviceCollection)
        {
            var rootNavigator = new Navigator(_logger);
            rootNavigator.AddPage(FindPage<Menu.HeroSelectionMenu>());
            rootNavigator.AddPage(FindPage<Battle.BattlePage>());
            serviceCollection.AddService<INavigator>(rootNavigator);
            return rootNavigator;
        }

        private T FindPage<T>() where T : NavigationPageBase
        {
            var page = Object.FindObjectOfType<T>(true);
            if(!page)
            {
                _logger.LogError($"Cannot start the game. No Page of type {typeof(T).Name} Found.");
                return null;
            }

            return page;
        }
    }
}
