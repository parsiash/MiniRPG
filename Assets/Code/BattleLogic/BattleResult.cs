namespace MiniRPG.BattleLogic
{
    public class BattleResult
    {
        public int winnerPlayerIndex { get; set; }
        public PlayerBattleResult[] playerResults { get; set; }

        public BattleResult(int winnerPlayerIndex, PlayerBattleResult[] playerResults)
        {
            this.winnerPlayerIndex = winnerPlayerIndex;
            this.playerResults = playerResults;
        }
    }

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

    public class UnitBattleResult
    {
        public int heroId { get; set; }
        public bool isAlive { get; set; }

        public UnitBattleResult(int heroId, bool isAlive)
        {
            this.heroId = heroId;
            this.isAlive = isAlive;
        }
    }
}