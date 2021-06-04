using System.Collections.Generic;
using System.Linq;

namespace MiniRPG.Metagame
{
    public class UserProfile
    {
        public string username { get; set; }
        public HeroData[] heroes { get; set; }
        public ProfileDeck deck { get; set; }
        public int battleCount { get; set; }

        public int HeroCount => heroes.Length;
        public int MaxHeroId
        {
            get
            {
                if(HeroCount == 0)
                {
                    return -1;
                }

                return heroes.Max(h => h.heroId);
            }
        }

        public int AverageHeroLevel
        {
            get
            {
                return (int)(heroes.Average(h => h.level));
            }
        }

        public UserProfile(string username, HeroData[] heroes, ProfileDeck deck, int battleCount)
        {
            this.username = username;
            this.heroes = heroes;
            this.deck = deck;
            this.battleCount = battleCount;
        }

        public HeroData GetHero(int heroId)
        {
            return heroes.FirstOrDefault(h => h.heroId == heroId);
        }

        public void AddHero(HeroData hero)
        {
            heroes = heroes.Append(hero).ToArray();
        }
    }
}