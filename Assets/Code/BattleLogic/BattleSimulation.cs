using System.Collections.Generic;
using System.Linq;
using MiniRPG.Common;

namespace MiniRPG.BattleLogic
{
    /// <summary>
    /// The simulation of battle logic is hanlded in an instance of this class.
    /// </summary>
    public class BattleSimulation
    {
        private const int DEFAULT_PLAYER_COUNT = 2;
        private Player[] _players;

        private int _turn;
        public int Turn => _turn;

        private IEntityFactory _entityFactory;
        private IEntityManager _entityManager;


        private ILogger _logger;

        public BattleSimulation(BattleInitData battleInitData, ILogger logger)
        {
            _entityFactory = new EntityFactory(this, logger);
            _entityManager = new EntityManager(logger);
            _logger = logger;

            //create players
            _players = battleInitData.players.Select(pid => CreatePlayer(pid)).ToArray();
        }

        private Unit CreateUnit(Player player, UnitInitData unitInitData)
        {
            var unit = _entityFactory.CreateEntity<Unit>(unitInitData.entityId);

            var unitStat = unitInitData.unitStat;
            unit.AddComponent(new HealthComponent(unitStat.health));
            unit.AddComponent(new AttackComponent(unitStat.attack));

            AddUnit(unit, player);

            return unit;
        }

        private Player CreatePlayer(PlayerInitData playerInitData)
        {
            //create player instance
            var player = new Player(playerInitData.index);

            //create player's initial units
            foreach(var unitInitData in playerInitData.units)
            {
                CreateUnit(player, unitInitData);
            }

            return player;
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

            return playerIndex == 0 ? _players[0] : _players[1];
        }

        public TurnResult PlayTurn(PlayTurnData data)
        {
            if(data.turn != _turn)
            {
                _logger.LogError($"Cannot play the turn action. Turn is invalid : {data.turn}");
                return null;
            }

            //retrieve the attacker unit
            var attacker = _entityManager.GetEntity(data.action.attackerId) as Unit;
            if(attacker == null)
            {
                _logger.LogError($"No Attacker found with entity id : {data.action.attackerId}");
                return null;
            }

            //check if it's the player's turn
            if(!IsPlayerTurn(attacker.PlayerIndex))
            {
                _logger.LogError($"Attack Failed for player : {attacker.PlayerIndex}. It is not your turn.");
                return null;
            }

            //retrieve the target unit
            var target = _entityManager.GetEntity(data.action.attackerId) as Unit;
            if(target == null)
            {
                _logger.LogError($"No target found with entity id : {data.action.targetId}");
                return null;
            }

            //compute and apply attack damage
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

        private void AddUnit(int playerIndex, Unit unit)
        {
            var player = GetPlayer(playerIndex);
            if(player == null)
            {
                _logger.LogError($"Cannot add unit. No Player found with index : {playerIndex}");
                return;
            }

            AddUnit(unit, player);
        }

        private void AddUnit(Unit unit, Player player)
        {
            _entityManager.AddEntity(unit);
            player.AddUnit(unit);
            unit.Init(player);
        }

        public void DestroyUnit(Unit unit)
        {
            _entityManager.RemoveEntity(unit.id);
            unit.player.RemoveUnit(unit.id);
        }

        private void ChangeTurn()
        {
            _turn++;
        }
    }
}