using MiniRPG.Common;

namespace MiniRPG.Metagame
{
    public class HeroData
    {
        public int heroId { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public MyColor color { get; set; }
        public float size { get; set; }

        public HeroData(int heroId, string name, int level, int experience, int attack, int health, MyColor color, float size)
        {
            this.heroId = heroId;
            this.name = name;
            this.level = level;
            this.experience = experience;
            this.attack = attack;
            this.health = health;
            this.color = color;
            this.size = size;
        }
    }
}