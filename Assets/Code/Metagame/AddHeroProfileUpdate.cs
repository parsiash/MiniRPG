namespace MiniRPG.Metagame
{
    public class IncrementHeroLevel : IProfileUpdate
    {
        public int heroId { get; private set; }
        public int amount { get; private set; }

        public IncrementHeroLevel(int heroId, int amount)
        {
            this.heroId = heroId;
            this.amount = amount;
        }

        public bool Apply(UserProfile profile)
        {
            var hero = profile.GetHero(heroId);
            if(hero == null)
            {
                return false;
            }

            for(int i = 0; i < amount; i++)
            {
                hero.level++;
                hero.attack = hero.attack + hero.attack / 10;
                hero.health = hero.health + hero.attack / 10;
            }

            return true;
        }
    }

    public class IncreaseHeroXP : IProfileUpdate
    {
        public int heroId { get; private set; }
        public int amount { get; private set; }

        public IncreaseHeroXP(int heroId, int amount)
        {
            this.heroId = heroId;
            this.amount = amount;
        }

        public bool Apply(UserProfile profile)
        {
            var hero = profile.GetHero(heroId);
            if(hero == null)
            {
                return false;
            }
            
            hero.experience += amount;
            return true;
        }
    }

    public class AddHero : IProfileUpdate
    {
        public ProfileHero hero { get; private set; }

        public AddHero(ProfileHero hero)
        {
            this.hero = hero;
        }

        public bool Apply(UserProfile profile)
        {
            profile.AddHero(hero);
            return true;
        }
    }
} 