using MiniRPG.BattleLogic;
using MiniRPG.BattleView;
using MiniRPG.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace MiniRPG.Battle
{
    public class BattleControllerConfiguration
    {
        public BattleInitData battleInitData { get; set; }
        public System.Action<BattleResult> OnBattleFinishCallback;

        public BattleControllerConfiguration(BattleInitData battleInitData, System.Action<BattleResult> onBattleFinishCallback)
        {
            this.battleInitData = battleInitData;
            OnBattleFinishCallback = onBattleFinishCallback;
        }
    }

    public interface IBattleActionListener
    {
        void OnAttack(int playerIndex, int attackerId, int targetId);
        void OnRandomAttack(int playerIndex, int attackerId);
    }

    /// <summary>
    /// The class responsible for managing the battle simulation and syncing it with battle view.
    /// </summary>
    public class BattleController : CommonBehaviour, IBattleActionListener
    {
        private IBattleSimulation _battleSimulation;
        private IBattleView battleView => RetrieveCachedComponentInChildren<BattleView.BattleView>();
        private IEntityViewFactory entityViewFactory
        {
            get
            {
                _entityViewFactory = _entityViewFactory ?? new EntityViewFactory(logger);
                return _entityViewFactory;
            }
        }
        private IEntityViewFactory _entityViewFactory;

        private BattleControllerConfiguration _configuration;

        public void Init(BattleControllerConfiguration configuration)
        {
            _configuration = configuration;
            logger.LogDebug("Battle init data : " + JsonConvert.SerializeObject(configuration.battleInitData));

            _battleSimulation = new BattleSimulation(configuration.battleInitData, logger);
            battleView.Init(_battleSimulation, entityViewFactory, this);
        }

        public void StartBattle()
        { 
            _battleSimulation.StartBattle();
        }

        public void OnAttack(int playerIndex, int attackerId, int targetId)
        {
            var turnResult = _battleSimulation.PlayTurn(
                new PlayTurnData(
                    _battleSimulation.Turn,
                    playerIndex,
                    new TurnActionData(
                        attackerId,
                        targetId
                    )
                )
            );

            if(turnResult != null)
            {
                foreach(var turnEvent in turnResult.events)
                {
                    string eventName = turnEvent.name;
                    switch(eventName)
                    {
                        case TurnEvent.ATTACK:
                            var attackResult = turnEvent.GetData<AttackResult>();
                            var attackerUnitView = battleView.GetEntityView(attackResult.attackerId) as UnitView;
                            var targetUnitView = battleView.GetEntityView(attackResult.targetId) as UnitView;
                            if(!targetUnitView)
                            {
                                logger.LogError($"Showing attack result failed. Target Unit View Not Found : {attackResult.targetId}");
                            }else
                            {
                                attackerUnitView.Attack(
                                    targetUnitView,
                                    () => targetUnitView.TakeDamage(attackResult.actualDamage)
                                );
                            }
                            break;

                        default:
                            logger.LogError($"Unhandled Turn Event : {eventName}");
                            break;
                    }
                }
            }

            //check battle finish
            if(_battleSimulation.IsFinished)
            {
                var battleResult = _battleSimulation.GetBattleResult();
                if(_configuration?.OnBattleFinishCallback != null)
                {
                    _configuration.OnBattleFinishCallback(battleResult);
                }
            }
        }

        public void OnRandomAttack(int playerIndex, int attackerId)
        {
            //get alive units of the opponent player
            var otherPlayer = _battleSimulation.GetOpponentPlayer(playerIndex);
            var aliveUnits = otherPlayer.AliveUnits;
            if(aliveUnits.Length == 0)
            {
                logger.LogError($"Random attack failed. Opponent has no alive units!");
                return;
            }

            //choose random target
            var randomTarget = aliveUnits[Random.Range(0, aliveUnits.Length)];
            
            OnAttack(playerIndex, attackerId, randomTarget.id);
        }

        public void Clear()
        {
            _configuration = null;
            battleView.Clear();
        }
    }
}                                                                                                                                      