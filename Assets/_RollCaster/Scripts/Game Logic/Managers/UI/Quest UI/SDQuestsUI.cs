using SD_GameLoad;
using SD_Core;
using UnityEngine;
using TMPro;
using System;
using SD_Quest;
using System.Collections;

namespace SD_UI
{
    /// <summary>
    /// Handles the User Interface interactions for quests in the game, including updating the quest description,
    /// opening and closing the quest tab, and displaying a toast message for quest rewards.
    /// </summary>
    public class SDQuestsUI : SDLogicMonoBehaviour
    {
        [SerializeField] TMP_Text questDescription;
        [SerializeField] GameObject questClosedTab;
        [SerializeField] GameObject questOpenedTab;
        [SerializeField] GameObject questToast;

        private void OnEnable()
        {
            AddListener(SDEventNames.UpdateQuest, UpdateQuestDescription);
            AddListener(SDEventNames.QuestToast, ToastQuestReward);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.UpdateQuest, UpdateQuestDescription);
            RemoveListener(SDEventNames.QuestToast, ToastQuestReward);
        }

        private void Start() => UpdateQuestDescription();

        public void OpenQuestTab() => SwitchQuestTab(true);

        public void CloseQuestTab() => SwitchQuestTab(false);

        private void UpdateQuestDescription(object obj = null) => questDescription.text = $"Defeat Boss Lv. {CalculateNextBossLevel()}";

        private void ToastQuestReward(object obj = null) => ShowRewardToast();

        /// <summary>
        /// Calculates the level of the next boss based on the current boss level.
        /// </summary>
        /// <returns>The level of the next boss.</returns>
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

        private void ShowRewardToast()
        {
            StartCoroutine(ShowAndHideToast());
        }

        /// <summary>
        /// Coroutine for showing the quest reward toast and then hiding it after a delay.
        /// </summary>
        /// <returns>An IEnumerator that can be used with StartCoroutine.</returns>
        private IEnumerator ShowAndHideToast()
        {
            questToast.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            questToast.SetActive(false);
        }
    }
}
