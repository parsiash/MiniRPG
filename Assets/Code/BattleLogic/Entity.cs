using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniRPG.BattleLogic
{
    public class Entity
    {
        public int id { get; private set; }
        public List<Component> components { get; private set; }

        public Entity(int id)
        {
            this.id = id;
        }

        public Component GetComponent(Type type)
        {
            return components.FirstOrDefault(c => c.GetType() == type);
        }

        public T GetComponent<T>() where T : Component
        {
            return GetComponent(typeof(T)) as T;
        }
    }
}