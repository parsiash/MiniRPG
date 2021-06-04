using System.Collections.Generic;

namespace MiniRPG.Logic.Battle
{
    /// <summary>
    /// The interface of Battle Simulation.
    /// </summary>
    public interface IBattleSimulation
    {
        BattleState State { get; }
        void StartBattle();
        int Turn { get; }
        bool IsPlayerTurn(int playerIndex);
        TurnResult PlayTurn(PlayTurnData data);
        IEnumerable<Entity> Entities { get; }
        Player GetPlayer(int playerIndex);
        bool IsFinished { get; }
        BattleResult GetBattleResult();
    }
        
}