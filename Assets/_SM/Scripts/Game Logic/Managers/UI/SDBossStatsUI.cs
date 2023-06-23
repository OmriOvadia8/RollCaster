using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SD_GameLoad;
using SD_Core;

namespace SD_UI
{
    public class SDBossStatsUI : SDLogicMonoBehaviour
    {
        [SerializeField] TMP_Text levelText;
        [SerializeField] TMP_Text hpText;
        [SerializeField] Image healthBar;

        private void OnEnable()
        {
            AddListener(SDEventNames.UpdateLevelUI, UpdateLevelText);
            AddListener(SDEventNames.UpdateHealthUI, UpdateHealthBar);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.UpdateLevelUI, UpdateLevelText);
            RemoveListener(SDEventNames.UpdateHealthUI, UpdateHealthBar);
        }

        private void Start()
        {
            UpdateLevelText();
            UpdateHealthBar();
        }

        private void UpdateLevelText(object obj = null) => levelText.text = $"Level {CurrentBossInfo.Level}";

        private void UpdateHealthBar(object obj = null)
        {
            double maxHealth = CurrentBossInfo.TotalHp;
            double currentHealth = CurrentBossInfo.CurrentHp;
            hpText.text = $"{currentHealth.ToReadableNumber()} / {maxHealth.ToReadableNumber()}";
            float fillAmount = (float)(currentHealth / maxHealth);
            healthBar.fillAmount = fillAmount;
        }
    }
}