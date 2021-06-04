using MiniRPG.Common;
using UnityEngine;

namespace MiniRPG.UI
{
    public class UIComponent : CommonBehaviour
    {
        public RectTransform rectTranform => RetrieveCachedComponent<RectTransform>();

        protected Canvas _parentCanvas;
        protected Canvas parentCanvas
        {
            get
            {
                if(!_parentCanvas)
                {
                    _parentCanvas = GetComponentInParent<Canvas>();
                }

                return _parentCanvas;
            }
        }

        protected Camera _mainCamera;
        protected Camera mainCamera
        {
            get
            {
                if(!_mainCamera)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

        public void SetWorldPosition(Vector3 worldPos)
        {
            //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
            Vector2 movePos;

            //Convert the screenpoint to ui rectangle local point
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);

            //Convert the local point to world point
            transform.position = parentCanvas.transform.TransformPoint(movePos);
        }
    }
}