using UnityEngine;

namespace MiniRPG.UI
{
    public interface IOnScreenMessageFactory
    {
        OnScreenMessage ShowMessage(OnScreenMessage.Configuration config);
    }

    public static class OnScreenMessageFactoryExtensions
    {
        public static OnScreenMessage ShowWarning(this IOnScreenMessageFactory factory, string warningMessage)
        {
            return factory.ShowMessage(
                new OnScreenMessage.Configuration(
                    warningMessage,
                    new Color(0.8f, 0.1f, 0.1f),
                    Vector2.zero
                )
            );
        }
    }

    public class OnScreenMessageFactory : IOnScreenMessageFactory
    {
        private ObjectPool<OnScreenMessage> _objectPool;

        public OnScreenMessageFactory(ObjectPool<OnScreenMessage> objectPool)
        {
            _objectPool = objectPool;
        }

        public OnScreenMessage ShowMessage(OnScreenMessage.Configuration config)
        {
            var onScreenMessage = _objectPool.RetrieveInstance(nameof(OnScreenMessage), true);
            onScreenMessage.Show(config, () => _objectPool.TryPoolInstance(nameof(OnScreenMessage), onScreenMessage));

            return onScreenMessage;
        }
    }
}