using System;

namespace MiniRPG.Logic.Battle
{
    public class TurnEvent
    {
        public const string ATTACK = "Attack";
        public string name;
        public System.Object data;

        public T GetData<T>() where T : class
        {
            return data as T;
        }

        public TurnEvent(string name, System.Object data = null)
        {
            this.name = name;
            this.data = data;
        }

        public Type DataType => data?.GetType();
    }
}