namespace MiniRPG.Logic.Battle
{
    public class AttackResult
    {
        public int attackerId { get; private set; }
        public int targetId { get; private set; }
        public int originalAttack { get; private set; }
        public int actualDamage { get; private set; }
        public int targetFinalHealth { get; private set; }

        public AttackResult(int attackerId, int targetId, int originalAttack, int actualDamage, int targetFinalHealth)
        {
            this.attackerId = attackerId;
            this.targetId = targetId;
            this.originalAttack = originalAttack;
            this.actualDamage = actualDamage;
            this.targetFinalHealth = targetFinalHealth;
        }
    }
}