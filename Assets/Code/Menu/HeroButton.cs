using MiniRPG.Common;
using MiniRPG.Metagame;
using MiniRPG.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MiniRPG.Menu
{
    public class HeroButtonConfiguration
    {
        public bool selected { get; set; }
        public HeroData hero { get; set; }
        public Action<HeroButton> OnClickCallback { get; set; }
        public Action<HeroButton> OnHoldCallback { get; set; }

        public HeroButtonConfiguration(bool selected, HeroData hero, Action<HeroButton> onClickCallback, Action<HeroButton> onHoldCallback)
        {
            this.selected = selected;
            this.hero = hero;
            OnClickCallback = onClickCallback;
            OnHoldCallback = onHoldCallback;
        }
    }

    public class HeroButton : UIComponent
    {
        private HeroData _hero;
        public HeroData Hero => _hero;
        private bool _selected;
        private Action<HeroButton> _onClickCallback;
        private Action<HeroButton> _onHoldCallback;

        [SerializeField] private Image heroImage;
        private Button button => RetrieveCachedComponentInChildren<Button>();

        private const string ANIM_PARAM_SELECTED = "Selected";
        private Animator animator => RetrieveCachedComponentInChildren<Animator>();

        public bool Selected
        {
            get
            {
                return _selected;       
            }

            set
            {
                _selected = value;
                animator?.SetBool(ANIM_PARAM_SELECTED, value);
            }
        }

        //@TODO : this is a hack and should be fixed by different before and after loaded event for INavigationPage
        void OnEnable()
        {
            Selected = Selected;
        }

        public void Init(HeroButtonConfiguration configuration)
        {
            _hero = configuration.hero;
            Selected = configuration.selected;
            _onClickCallback = configuration.OnClickCallback;
            _onHoldCallback = configuration.OnHoldCallback;

            heroImage.color = configuration.hero.color.GetUnityColor();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnButtonClick());
        }

        private void OnButtonClick()
        {
            if(_onClickCallback != null)
            {
                _onClickCallback(this);
            }
        }

        public void OnHold()
        {
            if(_onHoldCallback != null)
            {
                _onHoldCallback(this);
            }
        }
    }
}