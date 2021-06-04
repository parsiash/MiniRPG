namespace MiniRPG.Metagame
{
    public class IncreaseBattleCount : IProfileUpdate
    {
        public int amount { get; private set; }

        public IncreaseBattleCount(int amount)
        {
            this.amount = amount;
        }


        public bool Apply(UserProfile profile)
        {
            profile.battleCount += amount;
            return true;
        }
    }
} 