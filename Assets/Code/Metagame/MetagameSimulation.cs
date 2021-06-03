using System;
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
        private const int HERO_XP_TO_LEVEL = 5;
        private IUser _user;
        public IUser User => _user;

        private IProfileController _profileController;
        public IProfileController ProfileController => _profileController;

        private ILogger _logger;

        public MetagameSimulation(IUser user, IProfileController profileController, ILogger logger)
        {
            _user = user;
            _profileController = profileController;
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
                        _logger.Log($"Hero with name {hero.name} and id  {hero.heroId} got leveled up. \n new level : {hero.level}. new attack : {hero.attack}. new health: {hero.health}");

                        //handle hero level up
                        int heroTargetLevel = hero.experience / HERO_XP_TO_LEVEL + 1;
                        if (heroTargetLevel > hero.level)
                        {
                            _profileController.Update(new IncrementHeroLevel(hero.heroId, heroTargetLevel - hero.level));
                            _logger.Log($"Hero with name {hero.name} and id  {hero.heroId} got leveled up. \n new level : {hero.level}. new attack : {hero.attack}. new health: {hero.health}");
                        }
                    }
                }
            }
        }
    }
} 