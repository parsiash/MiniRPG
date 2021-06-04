using MiniRPG.Logic.Battle;
using MiniRPG.Common;

namespace MiniRPG.Logic.Metagame
{
    public class HeroData
    {
        public int heroId { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public UnitStat baseStat { get; set; }
        public UnitStat stat { get; set; }
        public MyColor color { get; set; }
        public float size { get; set; }

        public HeroData(int heroId, string name, int level, int experience, UnitStat baseStat, UnitStat stat, MyColor color, float size)
        {
            this.heroId = heroId;
            this.name = name;
            this.level = level;
            this.experience = experience;
            this.baseStat = baseStat;
            this.stat = stat;
            this.color = color;
            this.size = size;
        }
    }
}