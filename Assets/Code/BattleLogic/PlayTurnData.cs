namespace MiniRPG.BattleLogic
{
    public class PlayTurnData
    {
        public int turn;
        public int playerIndex;
        public TurnActionData action;
    }

    public class TurnActionData
    {
        public int attackerId;
        public int targetId;
    }
}