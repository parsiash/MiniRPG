using System;

namespace MiniRPG.Logic.Battle
{
    /// <summary>
    /// The base class of components that can be attached to entities in battle logic simulation.
    /// </summary>
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