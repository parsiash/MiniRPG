using MiniRPG.Common;
using MiniRPG.Logic.Metagame;
using MiniRPG.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MiniRPG.Menu
{
    /// <summary>
    /// The comnponent attached to Selectable hero buttons in Hero Selection Menu.
    /// </summary>
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

        private void OnEnable()
        {
            Selected = Selected;
        }
    }
}