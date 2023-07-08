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
        [SerializeField] TMP_Text extraRollsAmountText;
        [SerializeField] TMP_Text abilityPointsText;

        private void OnEnable()
        {
            AddListener(SDEventNames.UpdateXpUI, UpdateXpUI);
            AddListener(SDEventNames.UpdateRollsUI, UpdateRollsBar);
            AddListener(SDEventNames.UpdateAbilityPtsUI, UpdateAbilityPointsUI);
        }

        private void Start()
        {
            UpdateXpUI();
            UpdateRollsBar();
            UpdateAbilityPointsUI();
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.UpdateXpUI, UpdateXpUI);
            RemoveListener(SDEventNames.UpdateRollsUI, UpdateRollsBar);
            RemoveListener(SDEventNames.UpdateAbilityPtsUI, UpdateAbilityPointsUI);
        }

        private void UpdateXpUI(object obj = null)
        {
            var totalXP = GameLogic.Player.PlayerData.PlayerInfo.TotalXpRequired;
            var currentXP = GameLogic.Player.PlayerData.PlayerInfo.CurrentXp;
            var currentLevel = GameLogic.Player.PlayerData.PlayerInfo.Level;

            levelText.text = $"Lv. {currentLevel:N0}";
            xpBar.fillAmount = (float)(currentXP / totalXP);
            xpAmountText.text = $"{currentXP.ToReadableNumber()} / {totalXP.ToReadableNumber()}";
        }

        private void UpdateRollsBar(object obj = null)
        {
            var maxRolls = GameLogic.Player.PlayerData.PlayerInfo.MaxRolls;
            var currentRolls = GameLogic.Player.PlayerData.PlayerInfo.CurrentRolls;
            var extraRolls = GameLogic.Player.PlayerData.PlayerInfo.ExtraRolls;

            extraRollsAmountText.text = $"{extraRolls} Extra Rolls";
            rollsBar.fillAmount = (float)currentRolls / maxRolls;
            rollsAmountText.text = $"{currentRolls} / {maxRolls}";
        }

        private void UpdateAbilityPointsUI(object obj = null)
        {
            var currentAbilityPoints = GameLogic.Player.PlayerData.PlayerInfo.AbilityPoints;
            abilityPointsText.text = $"{currentAbilityPoints:N0}";
        }
    }
}