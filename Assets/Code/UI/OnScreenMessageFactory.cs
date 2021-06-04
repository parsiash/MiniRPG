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
        private OnScreenMessage _onScreenMessagePrefab;

        public OnScreenMessageFactory(OnScreenMessage onScreenMessagePrefab)
        {
            _onScreenMessagePrefab = onScreenMessagePrefab;
        }

        public OnScreenMessage ShowMessage(OnScreenMessage.Configuration config)
        {
            var onScreenMessage = GameObject.Instantiate<OnScreenMessage>(_onScreenMessagePrefab);
            onScreenMessage.Show(config);

            return onScreenMessage;
        }
    }
}