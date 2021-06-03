using UnityEngine;

namespace MiniRPG.UI
{
    public static class UIUtils
    {
        public static Vector2 GetWorldPosition(this UIComponent uiComponent)
        {
            return uiComponent.rectTranform.GetWorldPosition();
        }

        public static Vector2 GetWorldPosition(this RectTransform rect)
        {
            var corners = new Vector3[4];
            rect.GetWorldCorners(corners);

            return (corners[0] + corners[2]) / 2f;
        }
    }
}