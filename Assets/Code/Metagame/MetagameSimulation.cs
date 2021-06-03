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

            var playerResult = battleResult.playerResults[0];
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