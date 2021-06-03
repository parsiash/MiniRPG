namespace MiniRPG.Metagame
{
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
} 