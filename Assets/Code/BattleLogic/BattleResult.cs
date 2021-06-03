namespace MiniRPG.BattleLogic
{
    public class BattleResult
    {
        public int winnerPlayerIndex { get; set; }

        public BattleResult(int winnerPlayerIndex)
        {
            this.winnerPlayerIndex = winnerPlayerIndex;
        }
    }
}