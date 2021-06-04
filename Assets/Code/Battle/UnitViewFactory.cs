using MiniRPG.Common;
using UnityEngine;

namespace MiniRPG.BattleView
{
    public interface IUnitViewFactory
    {
        IUnitView CreateUnitView(string name);
        void DestroyUnitView(IUnitView unitView);
    }

    public class UnitViewFactory : IUnitViewFactory
    {
        private UnitView _unitViewPrefab;
        private Common.ILogger _logger;

        public UnitViewFactory(UnitView unitViewPrefab, Common.ILogger logger)
        {
            _unitViewPrefab = unitViewPrefab;
            _logger = logger;
        }

        public IUnitView CreateUnitView(string name)
        {
            return GameObject.Instantiate<UnitView>(_unitViewPrefab);
        }

        public void DestroyUnitView(IUnitView unitView)
        {
            if(unitView is UnitView)
            {
                GameObject.Destroy((unitView as CommonBehaviour).gameObject);
            }
        }
    }
}