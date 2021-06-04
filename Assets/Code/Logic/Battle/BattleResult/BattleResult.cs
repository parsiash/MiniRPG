namespace MiniRPG.Logic.Battle
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
}