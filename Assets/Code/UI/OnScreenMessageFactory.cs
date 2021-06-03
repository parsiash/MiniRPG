using UnityEngine;

namespace MiniRPG.UI
{
    public interface IOnScreenMessageFactory
    {
        OnScreenMessage ShowMessage(OnScreenMessage.Configuration config);
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