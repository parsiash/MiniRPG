using System;

namespace MiniRPG.BattleLogic
{
    public class HealthComponent : Component
    {
        public int health { get; private set; }
        public int maxHealth { get; private set; }
        public bool IsDead => health == 0;

        public HealthComponent(int maxHealth)
        {
            this.health = maxHealth;
            this.maxHealth = maxHealth;
        }

        public int TakeDamage(int damage)
        {
            var previousHealth = health;
            health = Math.Max(0, health - damage);

            return Math.Max(0, previousHealth - health);
        }
    }
}