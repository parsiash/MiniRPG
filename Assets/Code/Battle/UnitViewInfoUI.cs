using MiniRPG.Common;
using MiniRPG.UI;
using UnityEngine;

namespace MiniRPG.BattleView
{
    public class UnitViewInfoUI : CommonBehaviour
    {
        [SerializeField] private ProgressBar healthBar;

        public void Init(int maxHealth, int initialHealth)
        {
            SetVisible(true);
            healthBar.Init(maxHealth, initialHealth);
        }

        public void TakeDamage(int damage)
        {
            healthBar.Subtract(damage);
        }

        public void SetVisible(bool visible)
        {
            SetActive(visible);
        }
    }
}