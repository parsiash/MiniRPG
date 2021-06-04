namespace MiniRPG.Metagame
{
    public class AddHero : IProfileUpdate
    {
        public HeroData hero { get; private set; }

        public AddHero(HeroData hero)
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