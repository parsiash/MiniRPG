using MiniRPG.Logic.Battle;
using MiniRPG.BattleView;
using MiniRPG.Common;
using MiniRPG.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace MiniRPG.Battle
{
    /// <summary>
    /// The class responsible for managing the battle logic simulation and syncing it with battle view.
    /// </summary>
    public class BattleController : CommonBehaviour, IBattleActionListener
    {
        private BattleControllerConfiguration _configuration;
        public BattleControllerState State { get; private set; }

        private IBattleSimulation _battleSimulation;
        private IBattleView battleView => RetrieveCachedComponentInChildren<BattleView.BattleView>();


        public void Init(BattleControllerConfiguration configuration)
        {
            _configuration = configuration;
            logger.LogDebug("Battle init data : " + JsonConvert.SerializeObject(configuration.battleInitData));

            _battleSimulation = new BattleSimulation(configuration.battleInitData, logger);
            battleView.Init(_battleSimulation, configuration.unitViewFactory, this);

            State = BattleControllerState.Initial;
        }

        public void StartBattle()
        { 
            _battleSimulation.StartBattle();
            State = BattleControllerState.Started;
        }

        
        public void OnUnitViewClick(IUnitView unitView)
        {
            var playerIndex = unitView.Unit.PlayerIndex;
            if(playerIndex == 1)
            {
                _configuration.onScreenMessageFactory.ShowWarning("Click on your own units.");
                return;
            }

            OnRandomAttack(
                playerIndex,
                unitView.Unit.id
            );
        }

        public void OnUnitViewHold(IUnitView unitView)
        {
            _configuration.OnUnitHoldCallback(unitView);
        }
        
        public void OnRandomAttack(int playerIndex, int attackerId)
        {
            //get alive units of the opponent player
            var otherPlayer = _battleSimulation.GetOpponentPlayer(playerIndex);
            var aliveUnits = otherPlayer.AliveUnits;
            if(aliveUnits.Length == 0)
            {
                logger.LogDebug($"Random attack failed. Opponent has no alive units!");
                return;
            }

            //choose random target
            var randomTarget = aliveUnits[Random.Range(0, aliveUnits.Length)];
            
            OnAttack(playerIndex, attackerId, randomTarget.id);
        }

        public void OnAttack(int playerIndex, int attackerId, int targetId)
        {
            if(State != BattleControllerState.Started)
            {
                _configuration.onScreenMessageFactory.ShowWarning("Wait for your turn to play.");
                return;
            }

            if(!_battleSimulation.IsPlayerTurn(playerIndex))
            {
                _configuration.onScreenMessageFactory.ShowWarning("It is not your turn");
                return;
            }

            State = BattleControllerState.PlayingTurn;

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
                            OnAttackEvent(turnEvent);
                            break;

                        default:
                            logger.LogError($"Unhandled Turn Event : {eventName}");
                            break;
                    }
                }
            }
        }

        private void OnAttackEvent(TurnEvent turnEvent)
        {
            var attackResult = turnEvent.GetData<AttackResult>();
            var attackerUnitView = battleView.GetEntityView(attackResult.attackerId) as UnitView;
            var targetUnitView = battleView.GetEntityView(attackResult.targetId) as UnitView;
            if (!targetUnitView)
            {
                logger.LogError($"Showing attack result failed. Target Unit View Not Found : {attackResult.targetId}");
            }
            else
            {
                attackerUnitView.Attack(
                    targetUnitView,
                    () => OnAttackHit(targetUnitView, attackResult.actualDamage),
                    () => OnAttackFinished()
                );
            }
        }

        private void OnAttackFinished()
        {
            //check battle finish
            if (_battleSimulation.IsFinished)
            {
                FinishBattle();
                return;
            }

            State = BattleControllerState.Started;

            //play enemy turn
            if (_battleSimulation.IsPlayerTurn(1))
            {
                var aliveUnits = _battleSimulation.GetPlayer(1).AliveUnits;
                if (aliveUnits.Length > 0)
                {
                    OnRandomAttack(1, aliveUnits[0].id);
                }
            }
        }

        private void OnAttackHit(UnitView targetUnitView, int actualDamage)
        {
            targetUnitView.TakeDamage(actualDamage);
            _configuration.onScreenMessageFactory.ShowMessage(
                new OnScreenMessage.Configuration(
                    "-" + actualDamage,
                    Color.red,
                    targetUnitView.Position
                )
            );
        }

        private void FinishBattle()
        {
            var battleResult = _battleSimulation.GetBattleResult();
            if (_configuration?.OnBattleFinishCallback != null)
            {
                _configuration.OnBattleFinishCallback(battleResult);
            }
            State = BattleControllerState.Finished;
        }

        public void Clear()
        {
            _configuration = null;
            battleView.Clear();
        }

        
    }
}                                                                                                                                      