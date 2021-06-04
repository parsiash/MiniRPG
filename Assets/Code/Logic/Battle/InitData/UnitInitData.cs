using MiniRPG.Logic.Metagame;

namespace MiniRPG.Logic.Battle
{
    public class UnitInitData
    {
        public string name { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public UnitStat unitStat { get; set; }

        public HeroData hero { get; set; }

        /// <summary>
        /// This is an optional field for when we want to explicitly determine the entity id.
        /// E.g. For replaying the battle.
        /// </summary>
        public int entityId { get; set; }

        public UnitInitData(string name, int level, int experience, UnitStat unitStat, HeroData hero, int entityId = -1)
        {
            this.name = name;
            this.level = level;
            this.experience = experience;
            this.unitStat = unitStat;
            this.hero = hero;
            this.entityId = entityId;
        }

        public static UnitInitData CreateFromHeroData(HeroData hero)
        {
            return new UnitInitData(
                hero.name,
                hero.level,
                hero.experience,
                hero.stat,
                hero
            );
        }
    }

}