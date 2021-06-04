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
        private ObjectPool<UnitView> _unitViewPool;
        private Common.ILogger _logger;

        public UnitViewFactory(ObjectPool<UnitView> unitViewPool, Common.ILogger logger)
        {
            _unitViewPool = unitViewPool;
            _logger = logger;
        }

        public IUnitView CreateUnitView(string name)
        {
            return _unitViewPool.RetrieveInstance(nameof(UnitView), true);
        }

        public void DestroyUnitView(IUnitView unitView)
        {
            if(unitView is UnitView)
            {
                _unitViewPool.TryPoolInstance(nameof(UnitView), unitView as UnitView);
            }
        }
    }
}