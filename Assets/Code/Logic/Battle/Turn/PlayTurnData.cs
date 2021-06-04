namespace MiniRPG.Logic.Battle
{
    public class PlayTurnData
    {
        public int turn;
        public int playerIndex;
        public TurnActionData action;

        public PlayTurnData(int turn, int playerIndex, TurnActionData action)
        {
            this.turn = turn;
            this.playerIndex = playerIndex;
            this.action = action;
        }
    }

    public class TurnActionData
    {
        public int attackerId;
        public int targetId;

        public TurnActionData(int attackerId, int targetId)
        {
            this.attackerId = attackerId;
            this.targetId = targetId;
        }
    }
}