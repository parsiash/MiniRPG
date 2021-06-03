using UnityEngine;
using DG.Tweening;

namespace MiniRPG.UI
{
    public class OnScreenMessage : UIComponent
    {
        private CustomText customText => RetrieveCachedComponentInChildren<CustomText>();

        public void Show(string text, Color color, Vector2 worldPosition, float fadeTime = 2f, float scale = 1)
        {
            customText.text = text;
            customText.color = color;
            transform.position = worldPosition;
            transform.localScale = Vector3.one * scale;

            var sequence = DOTween.Sequence();
            sequence.Append(customText.DOFade(0, fadeTime));
            sequence.AppendCallback(() => GameObject.Destroy(gameObject));
        }
    }
}