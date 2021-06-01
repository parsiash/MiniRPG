using System;
using System.Collections.Generic;
using System.Linq;
using MiniRPG.Common;

namespace MiniRPG.BattleLogic
{
    public class Entity
    {
        public string name { get; private set; }
        public int id { get; private set; }
        public List<Component> components { get; private set; }

        protected ILogger logger;
        protected BattleSimulation battleSimulation;

        public Entity(string name, int id, BattleSimulation battleSimulation, ILogger logger)
        {
            this.id = id;
            this.name = name;
            this.logger = logger;
            this.battleSimulation = battleSimulation;
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
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