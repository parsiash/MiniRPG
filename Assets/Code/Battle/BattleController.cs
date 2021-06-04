using MiniRPG.BattleLogic;
using MiniRPG.BattleView;
using MiniRPG.Common;
using MiniRPG.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace MiniRPG.Battle
{
    public class BattleControllerConfiguration
    {
        public BattleInitData battleInitData { get; private set; }
        public System.Action<BattleResult> OnBattleFinishCallback { get; private set; }
        public System.Action<IUnitView> OnUnitHoldCallback { get; private set; }
        public IOnScreenMessageFactory onScreenMessageFactory { get; private set; }

        public BattleControllerConfiguration(BattleInitData battleInitData, System.Action<BattleResult> onBattleFinishCallback, System.Action<IUnitView> onUnitHoldCallback, IOnScreenMessageFactory onScreenMessageFactory)
        {
            this.battleInitData = battleInitData;
            OnBattleFinishCallback = onBattleFinishCallback;
            OnUnitHoldCallback = onUnitHoldCallback;
            this.onScreenMessageFactory = onScreenMessageFactory;
        }
    }

    public interface IBattleActionListener
    {
        void OnAttack(int playerIndex, int attackerId, int targetId);
        void OnRandomAttack(int playerIndex, int attackerId);
        void OnUnitViewHold(IUnitView unitView);
    }

    public enum BattleControllerState
    {
        Initial,
        Started,
        PlayingTurn,
        Finished
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

        public BattleControllerState State { get; private set; }

        public void Init(BattleControllerConfiguration configuration)
        {
            _configuration = configuration;
            logger.LogDebug("Battle init data : " + JsonConvert.SerializeObject(configuration.battleInitData));

            _battleSimulation = new BattleSimulation(configuration.battleInitData, logger);
            battleView.Init(_battleSimulation, entityViewFactory, this);

            State = BattleControllerState.Initial;
        }

        public void StartBattle()
        { 
            _battleSimulation.StartBattle();
            State = BattleControllerState.Started;
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
                                    () => {
                                        targetUnitView.TakeDamage(attackResult.actualDamage);
                                        _configuration.onScreenMessageFactory.ShowMessage(
                                            new OnScreenMessage.Configuration(
                                                "-" + attackResult.actualDamage,
                                                Color.red,
                                                targetUnitView.Position
                                            )
                                        );
                                    },
                                    () => {
                                        //check battle finish
                                        if(_battleSimulation.IsFinished)
                                        {
                                            FinishBattle();
                                            return;
                                        }

                                        State = BattleControllerState.Started;
                                        if(_battleSimulation.IsPlayerTurn(11))
                                        {
                                            var aliveUnits = _battleSimulation.GetPlayer(1).AliveUnits;
                                            if(aliveUnits.Length > 0)
                                            {
                                                OnRandomAttack(1, aliveUnits[0].id);
                                            }
                                        }
                                    }
                                );
                            }
                            break;

                        default:
                            logger.LogError($"Unhandled Turn Event : {eventName}");
                            break;
                    }
                }
            }
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

        public void OnUnitViewHold(IUnitView unitView)
        {
            _configuration.OnUnitHoldCallback(unitView);
        }
    }
}                                                                                                                                      