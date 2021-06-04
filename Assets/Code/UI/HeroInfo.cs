using MiniRPG.Metagame;

namespace MiniRPG.UI
{
    public class HeroInfo
    {
        public HeroInfoAttribute[] attributes { get; private set; }
        public int AttributesCount => attributes.Length;

        public HeroInfo(params HeroInfoAttribute[] attributes)
        {
            this.attributes = attributes;
        }

        public static HeroInfo CreateFromHero(HeroData hero)
        {
            return new HeroInfo(
                new HeroInfoAttribute("Name", hero.name),
                new HeroInfoAttribute("Level", hero.level),
                new HeroInfoAttribute("Experience", hero.experience),
                new HeroInfoAttribute("Attack", hero.attack),
                new HeroInfoAttribute("Health", hero.health)
            );
        }

        public HeroInfoAttribute GetAttribute(int index)
        {
            if(index >= 0 && index <= attributes.Length)
            {
                return attributes[index];
            }

            return null;
        }
    }
}