namespace MiniRPG.Logic.Battle
{
    public class PlayerBattleResult
    {
        public int index { get; set; }
        public UnitBattleResult[] unitResults { get; set; }

        public PlayerBattleResult(int index, UnitBattleResult[] unitResults)
        {
            this.index = index;
            this.unitResults = unitResults;
        }
    }
}