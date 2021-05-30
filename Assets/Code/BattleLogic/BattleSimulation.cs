using System;
using MiniRPG.Common;

namespace MiniRPG.BattleLogic
{
    /// <summary>
    /// The simulation of battle logic is hanlded in an instance of this class.
    /// </summary>
    public class BattleSimulation
    {
        /// <summary>
        /// The logger that is used in battle logic simulation.
        /// </summary>
        public ILogger logger => _logger;
        private ILogger _logger;

        private Player[] _players;

        private IEntityManager _entityManager;

        private int _turn;
        public int Turn => _turn;

        public BattleSimulation(BattleInitData battleInitData, ILogger logger)
        {
            _entityManager = new EntityManager(logger);

            //initialize battle
            _logger = logger;
        }

        public bool IsPlayerTurn(int playerIndex)
        {
            return playerIndex % 2 == _turn % 2;
        }

        public bool IsPlayerTurn(Player player)
        {
            return IsPlayerTurn(player.index);
        }

        public Player GetPlayer(int playerIndex)
        {
            if(playerIndex != 0 && playerIndex != 1)
            {
                _logger.LogError($"Invalid Player Index : {playerIndex}. Player Index should be 0 or 1.");
                return null;
            }
s
            return playerIndex == 0 ? _players[0] : _players[1];
        }

        public TurnResult PlayTurn(PlayTurnData data)
        {
            if(data.turn != _turn)
            {
                _logger.LogError($"Cannot play the turn action. Turn is invalid : {data.turn}");
                return null;
            }

            //check some other conditions and log accordingly

            var attacker = _entityManager.GetEntity(data.action.attackerId) as Unit;
            if(attacker == null)
            {
                _logger.LogError($"No Attacker found with entity id : {data.action.attackerId}");
                return null;
            }

            if(!IsPlayerTurn(attacker.PlayerIndex))
            {
                _logger.LogError($"Attack Failed for player : {attacker.PlayerIndex}. It is not your turn.");
                return null;
            }

            var target = _entityManager.GetEntity(data.action.attackerId) as Unit;
            if(target == null)
            {
                _logger.LogError($"No target found with entity id : {data.action.targetId}");
                return null;
            }

            int attack = attacker.attackComponent.Attack;
            var actualDamage = target.healthComponent.TakeDamage(attack);

            return new TurnResult(
                data.turn,
                new TurnEvent(
                    TurnEvent.ATTACK,
                    new AttackResult(
                        attacker.id,
                        target.id,
                        attack,
                        actualDamage,
                        target.healthComponent.health
                    )
                )
            );
        }
    }

    public class TurnResult
    {
        public int turn { get; set; }
        public TurnEvent[] events { get; set; }

        public TurnResult(int turn, params TurnEvent[] events)
        {
            this.turn = turn;
            this.events = events;
        }
    }


    public class TurnEvent
    {
        public const string ATTACK = "Attack";
        public string name;
        public System.Object data;

        public TurnEvent(string name, System.Object data = null)
        {
            this.name = name;
            this.data = data;
        }

        public Type DataType => data?.GetType();
    }

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


    public class PlayTurnData
    {
        public int turn;
        public int playerIndex;
        public TurnActionData action;
    }

    public class TurnActionData
    {
        public int attackerId;
        public int targetId;
    }
}