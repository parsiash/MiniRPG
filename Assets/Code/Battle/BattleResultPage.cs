using System;
using MiniRPG.Common;
using MiniRPG.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MiniRPG.Battle
{
    public class BattleResultPage : CommonBehaviour
    {
        public enum BattleResultStatus
        {
            Win,
            Lose
        }

        [SerializeField] private CustomText battleStatusText;
        [SerializeField] private Button finishBattleButton;


        public void ShowBattleResult(BattleResultStatus battleStatus, Action OnFinishCallback)
        {
            //init battle status text
            battleStatusText.text = battleStatus.ToString();

            //add finish button callback
            finishBattleButton.onClick.RemoveAllListeners();
            finishBattleButton.onClick.AddListener(() => OnFinishCallback());
            
            SetActive(true);
        }

        public void Hide()
        {
            SetActive(false);
        }
    }
}