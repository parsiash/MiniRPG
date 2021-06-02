using System.Threading.Tasks;
using MiniRPG.BattleLogic;
using MiniRPG.Common;
using MiniRPG.Navigation;

namespace MiniRPG.Battle
{
    public class BattlePage : NavigationPageBase
    {
        public class OnLoadData : INavigationData
        {
            public BattleInitData battleInitData { get; set ;}

            public OnLoadData(BattleInitData battleInitData)
            {
                this.battleInitData = battleInitData;
            }
        }

        public BattleController battleController => RetrieveCachedComponentInChildren<BattleController>();

        public override async Task<bool> OnLoaded(INavigator parentNavigator, INavigationData data)
        {
            await base.OnLoaded(parentNavigator, data);

            var loadData = data as OnLoadData;
            if(loadData == null)
            {
                throw new NavigationException($"Loading battle page failed. No load data is provided to {nameof(OnLoaded)} method.");
            }

            //init battle controller
            battleController.Init(new BattleControllerInitData(
                loadData.battleInitData
            ));

            return true;
        }
    }
}