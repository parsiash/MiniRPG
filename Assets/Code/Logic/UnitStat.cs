namespace MiniRPG.Logic
{
    /// <summary>
    /// The upgradable stat of the units. (heroes in battle)
    /// </summary>
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