namespace MiniRPG.Logic
{
    public class UnitStat
    {
        public int attack { get; set; }
        public int health { get; set; }

        public UnitStat(int attack, int health)
        {
            this.attack = attack;
            this.health = health;
        }
    }
}