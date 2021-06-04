namespace MiniRPG.Logic.Battle
{
    public class TurnResult
    {
        public int turn { get; set; }
        public TurnEvent[] events { get; set; }

        public TurnResult(int turn, params TurnEvent[] events)
        {
            this.turn = turn;
            this.events = events;
        }
    }
}