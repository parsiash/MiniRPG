namespace MiniRPG.Metagame
{
    public class UserProfile
    {
        public string username;
        public ProfileHero[] heroes;

        public UserProfile(string username, ProfileHero[] heroes)
        {
            this.username = username;
            this.heroes = heroes;
        }
    }
}