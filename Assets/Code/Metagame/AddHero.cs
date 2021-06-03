namespace MiniRPG.Metagame
{
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