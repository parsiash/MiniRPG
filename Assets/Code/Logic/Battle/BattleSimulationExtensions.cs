namespace MiniRPG.Logic.Battle
{
    public static class BattleSimulationExtensions
    {
        public static Player GetOpponentPlayer(this IBattleSimulation battleSimulation, int playerIndex)
        {
            return battleSimulation.GetPlayer(1 - playerIndex);
        }
    }
        
}