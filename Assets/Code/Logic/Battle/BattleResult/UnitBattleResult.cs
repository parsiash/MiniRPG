namespace MiniRPG.Logic.Battle
{
    public class UnitBattleResult
    {
        public int heroId { get; set; }
        public bool isAlive { get; set; }

        public UnitBattleResult(int heroId, bool isAlive)
        {
            this.heroId = heroId;
            this.isAlive = isAlive;
        }
    }
}