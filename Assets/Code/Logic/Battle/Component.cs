using System;

namespace MiniRPG.Logic.Battle
{
    public class Component
    {
        public Entity entity { get; private set; }

        public void SetEntity(Entity entity)
        {
            this.entity = entity;
        }
        public Component GetComponent(Type type)
        {
            return entity?.GetComponent(type);
        }

        public T GetComponent<T>() where T : Component
        {
            return entity?.GetComponent<T>();
        }
    }
}