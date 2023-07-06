using SD_GameLoad;
using SD_Core;
using UnityEngine;
using TMPro;
using System;
using SD_Quest;

namespace SD_UI
{
    public class SDQuestsUI : SDLogicMonoBehaviour
    {
        [SerializeField] TMP_Text questDescription;
        [SerializeField] GameObject questClosedTab;
        [SerializeField] GameObject questOpenedTab;

        private void OnEnable()
        {
            AddListener(SDEventNames.UpdateQuest, UpdateQuestDescription);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.UpdateQuest, UpdateQuestDescription);
        }

        private void Start() => UpdateQuestDescription();

        public void OpenQuestTab() => SwitchQuestTab(true);

        public void CloseQuestTab() => SwitchQuestTab(false);

        private void UpdateQuestDescription(object obj = null)
        {
            questDescription.text = $"Defeat Boss Lv. {CalculateNextBossLevel()}";
        }

        private int CalculateNextBossLevel()
        {
            int currentBossLevel = GameLogic.BossController.GetBossLevel();
            return (int)Math.Ceiling(currentBossLevel / (double)SDQuestsManager.BOSS_INTERVAL) * SDQuestsManager.BOSS_INTERVAL;
        }

        public void SwitchQuestTab(bool isOpen)
        {
            questClosedTab.SetActive(!isOpen);
            questOpenedTab.SetActive(isOpen);
        }
    }
}
