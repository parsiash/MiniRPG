namespace MiniRPG.BattleLogic
{
    /// <summary>
    /// The data needed for initialization of the battle.
    /// </summary>
    public class BattleInitData
    {
        public PlayerInitData[] players { get; set; }

        public BattleInitData(params PlayerInitData[] players)
        {
            this.players = players;
        }
    }

    public class PlayerInitData
    {
        public int index { get; set; }
        public UnitInitData[] units { get; set; }

        public PlayerInitData(int index, UnitInitData[] units)
        {
            this.index = index;
            this.units = units;
        }
    }

    public class UnitInitData
    {
        public string name { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public UnitStat unitStat { get; set; }

        public UnitInitData(string name, int level, int experience, UnitStat unitStat)
        {
            this.name = name;
            this.level = level;
            this.experience = experience;
            this.unitStat = unitStat;
        }
    }

}