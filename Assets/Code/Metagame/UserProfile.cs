using System.Linq;

namespace MiniRPG.Metagame
{
    public class UserProfile
    {
        public string username { get; set; }
        public ProfileHero[] heroes { get; set; }
        public ProfileDeck deck { get; set; }
        public int battleCount { get; set; }

        public int HeroCount => heroes.Length;
        public int MaxHeroId => heroes.Max(h => h.heroId);

        public UserProfile(string username, ProfileHero[] heroes, ProfileDeck deck, int battleCount)
        {
            this.username = username;
            this.heroes = heroes;
            this.deck = deck;
            this.battleCount = battleCount;
        }

        public ProfileHero GetHero(int heroId)
        {
            return heroes.FirstOrDefault(h => h.heroId == heroId);
        }

        public void AddHero(ProfileHero hero)
        {
            heroes = heroes.Append(hero).ToArray();
        }
    }
}