namespace MiniRPG.Logic.Battle
{
    public class AttackComponent : Component
    {
        public int Attack { get; private set; }

        public AttackComponent(int attack)
        {
            Attack  = attack;
        }
    }
}