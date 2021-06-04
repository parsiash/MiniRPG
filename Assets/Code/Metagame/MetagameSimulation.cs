using System.Linq;
using MiniRPG.BattleLogic;
using MiniRPG.Common;

namespace MiniRPG.Metagame
{
    public interface IMetagameSimulation
    {
        IUser User { get; }
        void OnBattleResult(BattleResult battleResult);
        void OnDeckChange(ProfileDeck newDeck);
        BattleInitData StartBattle();
    }

    public class MetagameSimulation : IMetagameSimulation
    {
        private const int MAX_HERO_COUNT = 10;
        private const int NEW_HERO_BATTLE_COUNT = 5;
        private const int HERO_XP_TO_LEVEL = 5;
        private IUser _user;
        public IUser User => _user;

        private IProfileController _profileController;
        public IProfileController ProfileController => _profileController;

        private IHeroDataSource _heroDataSource;
        private IUnitStatProvider _unitStatProvider;

        private ILogger _logger;

        public MetagameSimulation(IUser user, IProfileController profileController, IHeroDataSource heroDataSource, IUnitStatProvider unitStatProvider, ILogger logger)
        {
            _user = user;
            _profileController = profileController;
            _heroDataSource = heroDataSource;
            _unitStatProvider = unitStatProvider;
            _logger = logger;
        }

        public void OnDeckChange(ProfileDeck newDeck)
        {
            var profileUpdate = new ChangeDeck(newDeck); 
            _profileController.Update(profileUpdate);
        }

        public BattleInitData StartBattle()
        {
            var profile = _user.Profile;
            
            return new BattleLogic.BattleInitData(
                new PlayerInitData(
                    0,
                    profile.deck.heroIds.Select(hid => profile.GetHero(hid)).Select(
                        hero => UnitInitData.CreateFromHeroData(hero)
                    ).ToArray()
                ),
                new PlayerInitData(
                    1,
                    new UnitInitData[] { UnitInitData.CreateFromHeroData(_heroDataSource.GetRandomEnemy(profile.AverageHeroLevel * 2)) }
                )
            );
        }

        public void OnBattleResult(BattleResult battleResult)
        {
            var profile = _user.Profile;

            //acquire new hero every 5th battle
            profile.battleCount++;
            if(profile.HeroCount < MAX_HERO_COUNT && profile.battleCount % NEW_HERO_BATTLE_COUNT == 0)
            {
                var newHero = _heroDataSource.GetRandomHero(profile.MaxHeroId + 1);
                bool success = _profileController.Update(new AddHero(newHero));
                if(success)
                {
                    _logger.Log($"New hero acquired \n name : {newHero.name} - id : {newHero.heroId} - experience : {newHero.experience}");
                }
            }

            //experience and level up hanlding
            var playerResult = battleResult.playerResults[0];
            if(battleResult.winnerPlayerIndex == 0)
            {
                foreach(var unitResult in playerResult.unitResults)
                {
                    if(unitResult.isAlive)
                    {
                        var hero = profile.GetHero(unitResult.heroId);
                        if(hero == null)
                        {
                            _logger.LogError($"No hero with hero id : {unitResult.heroId} found in player's profile.");
                            continue;
                        }

                        //increase hero xp
                        _profileController.Update(new IncreaseHeroXP(unitResult.heroId, 1));
                        _logger.Log($"Hero with name {hero.name} and id  {hero.heroId} got leveled up. \n new level : {hero.level}. new attack : {hero.stat.attack}. new health: {hero.stat.health}");

                        //handle hero level up
                        int heroTargetLevel = hero.experience / HERO_XP_TO_LEVEL + 1;
                        if (heroTargetLevel > hero.level)
                        {
                            var oldStat = hero.stat;
                            var newStat = _unitStatProvider.GetUnitStatByLevel(hero.baseStat, heroTargetLevel);

                            _profileController.Update(
                                new IncrementHeroLevel(
                                    hero.heroId,
                                    heroTargetLevel - hero.level,
                                    newStat.attack - oldStat.attack,
                                    newStat.health - oldStat.health
                                )
                            );
                            _logger.Log($"Hero with name {hero.name} and id  {hero.heroId} got leveled up. \n new level : {hero.level}. new attack : {hero.stat.attack}. new health: {hero.stat.health}");
                        }
                    }
                }
            }
        }
    }
} 