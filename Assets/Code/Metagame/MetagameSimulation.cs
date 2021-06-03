using MiniRPG.BattleLogic;
using MiniRPG.Common;

namespace MiniRPG.Metagame
{
    public interface IMetagameSimulation
    {
        IUser User { get; }
        void OnBattleResult(BattleResult battleResult);
    }

    public class MetagameSimulation : IMetagameSimulation
    {
        private const int MAX_HERO_COUNT = 10;
        private const int NEW_HERO_BATTLE_COUNT = 5;
        private IUser _user;
        public IUser User => _user;

        private ILogger _logger;

        public MetagameSimulation(IUser user, ILogger logger)
        {
            _user = user;
            _logger = logger;
        }

        public void OnBattleResult(BattleResult battleResult)
        {
            var profile = _user.Profile;

            //acquire new hero every 5th battle
            profile.battleCount++;
            if(profile.HeroCount < MAX_HERO_COUNT && profile.battleCount % NEW_HERO_BATTLE_COUNT == 0)
            {
                var newHero = GameManager.Instance.GenerateHero(profile.MaxHeroId + 1);
                profile.AddHero(newHero);

                _logger.Log($"New hero acquired \n name : {newHero.name} - id : {newHero.heroId} - experience : {newHero.experience}");
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
                        if(hero != null)
                        {
                            hero.experience++;
                            _logger.Log($"Hero with name {hero.name} and id  {hero.heroId} experience increased to {hero.experience}");

                            var previousHeroLevel = hero.level;
                            hero.level = 1 + hero.experience / 5;
                            if(hero.level > previousHeroLevel)
                            {
                                hero.attack = hero.attack + hero.attack / 10;
                                hero.health = hero.health + hero.attack / 10;

                                _logger.Log($"Hero with name {hero.name} and id  {hero.heroId} got leveled up. \n new level : {hero.level}. new attack : {hero.attack}. new health: {hero.health}");
                            }
                        }
                    }
                }
            }
        }
    }
} 