using UnityEngine;
using SD_GameLoad;
using UnityEngine.UI;
using TMPro;
using SD_Core;

namespace SD_UI
{
    public class SDPlayerStatsUIController : SDLogicMonoBehaviour
    {
        [SerializeField] TMP_Text levelText;
        [SerializeField] Image xpBar;
        [SerializeField] TMP_Text xpAmountText;
        [SerializeField] Image rollsBar;
        [SerializeField] TMP_Text rollsAmountText;

        private void OnEnable()
        {
            AddListener(SDEventNames.UpdateXpUI, UpdateXpUI);
            AddListener(SDEventNames.UpdateRollsUI, UpdateRollsBar);
        }

        private void Start()
        {
            UpdateXpUI();
            UpdateRollsBar();
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.UpdateXpUI, UpdateXpUI);
            RemoveListener(SDEventNames.UpdateRollsUI, UpdateRollsBar);
        }

        private void UpdateXpUI(object obj = null)
        {
            var totalXP = GameLogic.Player.PlayerData.PlayerInfo.TotalXpRequired;
            var currentXP = GameLogic.Player.PlayerData.PlayerInfo.CurrentXp;
            var currentLevel = GameLogic.Player.PlayerData.PlayerInfo.Level;

            levelText.text = $"Lv. {currentLevel}";
            xpBar.fillAmount = (float)(currentXP / totalXP);
            xpAmountText.text = $"{currentXP} / {totalXP}";
        }

        private void UpdateRollsBar(object obj = null)
        {
            var maxRolls = GameLogic.Player.PlayerData.PlayerInfo.MaxRolls;
            var currentRolls = GameLogic.Player.PlayerData.PlayerInfo.CurrentRolls;

            rollsBar.fillAmount = (float)currentRolls / maxRolls;
            rollsAmountText.text = $"{currentRolls} / {maxRolls}";
        }
    }
}