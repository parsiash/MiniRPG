using System.Linq;

namespace MiniRPG.Metagame
{
    public class ProfileDeck
    {
        public const int MAX_HERO_COUNT = 3;
        public int[] heroIds { get; set; }
        public int HeroCount => heroIds.Length;
        public bool HasCapacity => HeroCount < MAX_HERO_COUNT;
        public bool IsEmpty => HeroCount == 0;

        public ProfileDeck()
        {
            this.heroIds = new int[]{};
        }

        public ProfileDeck(int[] heroIds)
        {
            this.heroIds = heroIds;
        }

        public bool ContainsHero(int heroId)
        {
            return heroIds.Any(hid => hid == heroId);
        }

        public void AddHero(int heroId)
        {
            heroIds = heroIds.Append(heroId).ToArray();
        }

        public void RemoveHero(int heroId)
        {
            heroIds = heroIds.Where(hid => hid != heroId).ToArray();
        }
    }
}