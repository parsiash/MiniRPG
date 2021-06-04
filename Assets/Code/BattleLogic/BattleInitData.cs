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
}