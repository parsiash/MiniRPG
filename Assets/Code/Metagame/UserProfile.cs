using System.Linq;

namespace MiniRPG.Metagame
{
    public class UserProfile
    {
        public string username;
        public ProfileHero[] heroes;
        public ProfileDeck deck;

        public UserProfile(string username, ProfileHero[] heroes, ProfileDeck deck)
        {
            this.username = username;
            this.heroes = heroes;
            this.deck = deck;
        }

        public ProfileHero GetHero(int heroId)
        {
            return heroes.FirstOrDefault(h => h.heroId == heroId);
        }
    }
}