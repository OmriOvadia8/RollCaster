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
        [SerializeField] GameObject bossCrown;

        private void OnEnable()
        {
            AddListener(SDEventNames.UpdateLevelUI, UpdateLevelText);
            AddListener(SDEventNames.UpdateHealthUI, UpdateHealthBar);
            AddListener(SDEventNames.BossCrownVisibility, BossCrownVisibility);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.UpdateLevelUI, UpdateLevelText);
            RemoveListener(SDEventNames.UpdateHealthUI, UpdateHealthBar);
            RemoveListener(SDEventNames.BossCrownVisibility, BossCrownVisibility);
        }

        private void Start()
        {
            UpdateLevelText();
            UpdateHealthBar();
            BossCrownVisibility();
        }

        private void UpdateLevelText(object obj = null) => levelText.text = $"Boss Lv. {CurrentBossInfo.Level}";

        private void UpdateHealthBar(object obj = null)
        {
            var maxHealth = CurrentBossInfo.TotalHp;
            var currentHealth = CurrentBossInfo.CurrentHp;
            hpText.text = $"{currentHealth.ToReadableNumber()} / {maxHealth.ToReadableNumber()}";
            var fillAmount = (float)(currentHealth / maxHealth);
            healthBar.fillAmount = fillAmount;
        }

        private void BossCrownVisibility(object obj = null)
        {
            bossCrown.gameObject.SetActive(false);

            if (CurrentBossInfo.Level % CurrentBossInfo.SpecialBossLevel == 0)
            {
                bossCrown.SetActive(true);
            }
        }
    }
}