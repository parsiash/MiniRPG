using MiniRPG.BattleLogic;
using MiniRPG.BattleView;
using MiniRPG.Common;
using Newtonsoft.Json;

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

    /// <summary>
    /// The class responsible for managing the battle simulation and syncing it with battle view.
    /// </summary>
    public class BattleController : CommonBehaviour
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
            battleView.Init(_battleSimulation, entityViewFactory);
        }

        public void StartBattle()
        { 
            _battleSimulation.StartBattle();
        }
    }
}                                                                                                                                      