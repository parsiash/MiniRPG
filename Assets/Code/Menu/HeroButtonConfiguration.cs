using MiniRPG.Logic.Metagame;
using System;

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
}