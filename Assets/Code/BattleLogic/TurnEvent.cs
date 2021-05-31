using System;

namespace MiniRPG.BattleLogic
{
    public class TurnEvent
    {
        public const string ATTACK = "Attack";
        public string name;
        public System.Object data;

        public TurnEvent(string name, System.Object data = null)
        {
            this.name = name;
            this.data = data;
        }

        public Type DataType => data?.GetType();
    }
}