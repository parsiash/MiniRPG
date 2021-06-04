using UnityEngine;
using DG.Tweening;

namespace MiniRPG.UI
{
    public class OnScreenMessage : UIComponent
    {
        public class Configuration
        {
            public string text { get; set; }
            public Color color { get; set; }
            public Vector2 worldPosition { get; set; }
            public float fadeTime { get; set; }
            public float scale { get; set; }
            public float delay { get; set; }

            public Configuration(string text, Color color, Vector2 worldPosition, float fadeTime = 2, float scale = 1, float delay = 0f)
            {
                this.text = text;
                this.color = color;
                this.worldPosition = worldPosition;
                this.fadeTime = fadeTime;
                this.scale = scale;
                this.delay = delay;
            }
        }

        private CustomText customText => RetrieveCachedComponentInChildren<CustomText>();

        public void Show(Configuration config, System.Action OnDestroyCallback)
        {
            SetActive(false);

            //intialize text and transform
            customText.text = config.text;
            customText.color = config.color;
            transform.position = config.worldPosition;
            transform.localScale = Vector3.one * config.scale;

            //animate text fade with tween
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(config.delay);
            sequence.AppendCallback(() => SetActive(true));
            sequence.Append(customText.DOFade(0, config.fadeTime));
            sequence.AppendCallback(() => OnDestroyCallback());

            transform.DOMoveY(transform.position.y + 1, 1);
        }

    }
}