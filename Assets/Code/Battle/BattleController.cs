using MiniRPG.BattleLogic;
using MiniRPG.BattleView;
using MiniRPG.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace MiniRPG.Battle
{
    public class BattleControllerInitData
    {
        public BattleInitData battleInitData { get; set; }

        public BattleControllerInitData(BattleInitData battleInitData)
        {
            this.battleInitData = battleInitData;
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

        public void Init(BattleControllerInitData initData)
        {
            logger.Log("Battle Controller init with data : " + JsonConvert.SerializeObject(initData));
            _battleSimulation = new BattleSimulation(initData.battleInitData, logger);
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
                            var targetUnitView = battleView.GetEntityView(attackResult.targetId) as UnitView;
                            if(!targetUnitView)
                            {
                                logger.LogError($"Showing attack result failed. Target Unit View Not Found : {attackResult.targetId}");
                            }else
                            {
                                targetUnitView.TakeDamage(attackResult.actualDamage);
                            }
                            break;

                        default:
                            logger.LogError($"Unhandled Turn Event : {eventName}");
                            break;
                    }
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
    }
}                                                                                                                                      