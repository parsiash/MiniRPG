using System.Collections.Generic;
using System.Linq;
using MiniRPG.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MiniRPG.Menu
{
    public class HeroListPanel : UIComponent
    {
        private List<HeroButton> _heroButtons;
        private List<HeroButton> heroButtons
        {
            get
            {
                _heroButtons = _heroButtons ?? new List<HeroButton>();
                return _heroButtons;
            }
        }

        [SerializeField] private HeroButton heroButtonPrefab;
        [SerializeField] private RectTransform buttonsLayoutRoot;

        public void InitHeroes(IEnumerable<HeroButtonConfiguration> buttonConfigurations)
        {
            Clear();

            foreach(var buttonConfiguration in buttonConfigurations)
            {
                var heroButton = GameObject.Instantiate<HeroButton>(heroButtonPrefab, buttonsLayoutRoot);
                heroButton.Init(buttonConfiguration);
                heroButton.transform.localScale = Vector3.one;

                heroButtons.Add(heroButton);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsLayoutRoot);
        }

        public HeroButton GetHeroButton(int heroId)
        {
            return heroButtons.FirstOrDefault(heroButton => heroButton.Hero.heroId == heroId);
        }

        public void Clear()
        {
            foreach(var heroButton in heroButtons)
            {
                GameObject.Destroy(heroButton.gameObject);
            }

            heroButtons.Clear();
        }
    }
}