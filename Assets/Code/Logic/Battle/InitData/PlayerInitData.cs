namespace MiniRPG.Logic.Battle
{
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

}