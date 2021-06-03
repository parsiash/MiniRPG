using MiniRPG.Common;
using UnityEngine;

namespace MiniRPG.UI
{
    public class UIComponent : CommonBehaviour
    {
        public RectTransform rectTranform => RetrieveCachedComponent<RectTransform>();
    }
}