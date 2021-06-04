using System.Linq;
using MiniRPG.BattleView;
using MiniRPG.Common;
using MiniRPG.Metagame;
using MiniRPG.Navigation;
using MiniRPG.UI;
using UnityEngine;

namespace MiniRPG
{
    public interface IServiceInitializer
    {
        void ConfigureServices();
    }

    public class ServiceInitializer : IServiceInitializer
    {
        private IServiceCollection _serviceCollection;
        private Common.ILogger _logger;

        public ServiceInitializer(IServiceCollection serviceCollection, Common.ILogger logger)
        {
            _serviceCollection = serviceCollection;
            _logger = logger;
        }

        public void ConfigureServices()
        {
            INavigator rootNavigator = ConfigureRootNavigator();

            IUnitStatProvider unitStatProvider = ConfigureUnitStatProvider();
            IHeroDataSource heroDataSource = ConfigureHeroDataSource(unitStatProvider);
            IPlayerDataRepository playerDataRepository = ConfigurePlayerDataRepository(heroDataSource);
            IProfileController profileController = ConfigureProfileController(playerDataRepository);

            IHeroAnouncementHandler heroAnouncementHandler = ConfigureHeroAnouncementHandler(profileController);
            HeroInfoPopup heroInfoPopup = ConfigureHeroInfoPopup();

            IUnitViewFactory unitViewFactory = ConfigureUnitViewFactory();
            IOnScreenMessageFactory onScreenMessageFactory = ConfigureOnScreenMessageFactory();

            IMetagameSimulation metagameSimulation = ConfigureMetagameSimulation(profileController, heroDataSource, unitStatProvider);
            IMenuLoader rootMenuLoader = ConfigureRootMenuLoader(rootNavigator, heroAnouncementHandler, onScreenMessageFactory, heroInfoPopup, metagameSimulation, unitViewFactory);
        }

        private IUnitStatProvider ConfigureUnitStatProvider()
        {
            IUnitStatProvider unitStatProvider = new UnitStatProvider();
            _serviceCollection.AddService<IUnitStatProvider>(unitStatProvider);
            return unitStatProvider;
        }

        private IMenuLoader ConfigureRootMenuLoader(INavigator rootNavigator, IHeroAnouncementHandler heroAnouncementHandler, IOnScreenMessageFactory onScreenMessageFactory, HeroInfoPopup heroInfoPopup, IMetagameSimulation metagameSimulation, IUnitViewFactory unitViewFactory)
        {
            IMenuLoader rootMenuLoader = new RootMenuLoader(
                rootNavigator,
                metagameSimulation,
                heroAnouncementHandler,
                onScreenMessageFactory,
                heroInfoPopup,
                unitViewFactory
            );
            _serviceCollection.AddService<IMenuLoader>(rootMenuLoader);
            return rootMenuLoader;
        }

        private IUnitViewFactory ConfigureUnitViewFactory()
        {
            var unitViewPrefab = Resources.Load<UnitView>("Entities/UnitView");
            var unitViewFactory = new UnitViewFactory(unitViewPrefab, _logger);
            _serviceCollection.AddService<IUnitViewFactory>(unitViewFactory);

            return unitViewFactory;
        }

        private IMetagameSimulation ConfigureMetagameSimulation(IProfileController profileController, IHeroDataSource heroDataSource, IUnitStatProvider unitStatProvider)
        {
            IMetagameSimulation metagameSimulation =
                new MetagameSimulation(
                    new User(
                        profileController.Profile
                    ),
                    profileController,
                    heroDataSource,
                    unitStatProvider,
                    _logger
                );
            _serviceCollection.AddService<IMetagameSimulation>(metagameSimulation);
            return metagameSimulation;
        }

        private HeroInfoPopup ConfigureHeroInfoPopup()
        {
            var heroInfoPopup = Object.FindObjectOfType(typeof(HeroInfoPopup), true) as HeroInfoPopup;
            _serviceCollection.AddService<HeroInfoPopup>(heroInfoPopup);
            return heroInfoPopup;
        }

        private IOnScreenMessageFactory ConfigureOnScreenMessageFactory()
        {
            var onScreenMessagePrefab = Resources.Load<OnScreenMessage>("OnScreenMessage");
            IOnScreenMessageFactory onScreenMessageFactory = new OnScreenMessageFactory(onScreenMessagePrefab);
            _serviceCollection.AddService<IOnScreenMessageFactory>(onScreenMessageFactory);
            return onScreenMessageFactory;
        }

        private IHeroAnouncementHandler ConfigureHeroAnouncementHandler(IProfileController profileController)
        {
            var heroAnouncementHandler = new HeroAnouncementHandler(_logger);
            profileController.AddListener(heroAnouncementHandler);
            _serviceCollection.AddService<IHeroAnouncementHandler>(heroAnouncementHandler);

            return heroAnouncementHandler;
        }

        private IProfileController ConfigureProfileController(IPlayerDataRepository playerDataRepository)
        {
            var profileController = new ProfileController(playerDataRepository.LoadUserProfile(), playerDataRepository, _logger);
            _serviceCollection.AddService<IProfileController>(profileController);
            return profileController;
        }

        private IPlayerDataRepository ConfigurePlayerDataRepository(IHeroDataSource heroDataSource)
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

            _serviceCollection.AddService<IPlayerDataRepository>(playerDataRepository);
            return playerDataRepository;
        }

        private IHeroDataSource ConfigureHeroDataSource(IUnitStatProvider unitStatProvider)
        {
            IHeroDataSource heroDataSource = new HeroDataSource(unitStatProvider, HeroTemplatesAsset.Instance, _logger);
            _serviceCollection.AddService<IHeroDataSource>(heroDataSource);
            return heroDataSource;
        }

        private INavigator ConfigureRootNavigator()
        {
            var rootNavigator = new Navigator(_logger);
            rootNavigator.AddPage(FindPage<Menu.HeroSelectionMenu>());
            rootNavigator.AddPage(FindPage<Battle.BattlePage>());
            _serviceCollection.AddService<INavigator>(rootNavigator);
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
