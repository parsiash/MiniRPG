namespace MiniRPG.Logic.Metagame
{
    public class IncrementHeroLevel : IProfileUpdate
    {
        public int heroId { get; private set; }
        public int levelIncrease { get; private set; }
        public int attackIncrease { get; private set; }
        public int healthIncrease { get; private set; }
        public IncrementHeroLevel(int heroId, int levelIncrease, int attackIncrease, int healthIncrease)
        {
            this.heroId = heroId;
            this.levelIncrease = levelIncrease;
            this.attackIncrease = attackIncrease;
            this.healthIncrease = healthIncrease;
        }

        public bool Apply(UserProfile profile)
        {
            var hero = profile.GetHero(heroId);
            if(hero == null)
            {
                return false;
            }

            hero.level += levelIncrease;
            hero.stat.attack += attackIncrease;
            hero.stat.health += healthIncrease;

            return true;
        }
    }
} 