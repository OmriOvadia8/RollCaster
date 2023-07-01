using SD_GameLoad;
using TMPro;
using UnityEngine;
using SD_Core;
using System;
using UnityEngine.UI;

namespace SD_UI
{
    public class SDAbilityInfoUI : SDLogicMonoBehaviour
    {
        [SerializeField] TMP_Text[] abilityLevelText;
        [SerializeField] TMP_Text[] abilityDamageText;
        [SerializeField] TMP_Text[] abilityUpgradeCostText;
        [SerializeField] TMP_Text[] abilityUnlockLevelText;
        [SerializeField] Button[] abilityUpgraderButton;
        [SerializeField] Image[] abilityIcon;
        [SerializeField] GameObject[] lockIcon;

        private void OnEnable()
        {
            AddListener(SDEventNames.UpdateAbilityUpgradeUI, UpdateUpgradeAbilityUI);
            AddListener(SDEventNames.UpdateAbilityUnlockedUI, UpdateUnlockedAbilityUI);
            AddListener(SDEventNames.UpdateAllUpgradesButtons, UpdateAllButtonsInteractability);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.UpdateAbilityUpgradeUI, UpdateUpgradeAbilityUI);
            RemoveListener(SDEventNames.UpdateAbilityUnlockedUI, UpdateUnlockedAbilityUI);
            RemoveListener(SDEventNames.UpdateAllUpgradesButtons, UpdateAllButtonsInteractability);
        }

        void Start() => UpdateAbilityTabsUI();

        private void UpdateUpgradeAbilityUI(object abilityData)
        {
            AbilityNames abilityName = (AbilityNames)abilityData;
            int index = (int)abilityName;
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName.ToString());

            if (ability != null)
            {
                abilityLevelText[index].text = $"Lv. {ability.Level:N0}";
                abilityDamageText[index].text = $"Dmg. {ability.Damage.ToReadableNumber()}";
                abilityUpgradeCostText[index].text = $"{ability.UpgradeCost:N0}";
                UpdateAllButtonsInteractability();
            }
        }

        private void UpdateUnlockedAbilityUI(object abilityData)
        {
            int currentPoints = GameLogic.PlayerController.GetAbilityPointsAmount();
            AbilityNames abilityName = (AbilityNames)abilityData;
            int index = (int)abilityName;
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName.ToString());

            if (ability != null)
            {
                abilityUnlockLevelText[index].text = $"UNLOCKS AT\nLV. {ability.UnlockLevel}";
                abilityUnlockLevelText[index].gameObject.SetActive(!ability.IsUnlocked);
                abilityIcon[index].color = ability.IsUnlocked ? Color.white : Color.black;
                lockIcon[index].SetActive(!ability.IsUnlocked);

                UpdateButtonInteractability(ability, index, currentPoints);
            }

            SDDebug.Log($"{ability}'s unlock is {ability.IsUnlocked}");
        }

        private void UpdateAllButtonsInteractability(object obj = null)
        {
            int currentPoints = GameLogic.PlayerController.GetAbilityPointsAmount();

            foreach (AbilityNames abilityName in Enum.GetValues(typeof(AbilityNames)))
            {
                int index = (int)abilityName;
                var ability = GameLogic.AbilityData.FindAbilityByName(abilityName.ToString());

                if (ability != null)
                {
                    UpdateButtonInteractability(ability, index, currentPoints);
                }
            }
        }

        private void UpdateAbilityTabsUI()
        {
            foreach (AbilityNames abilityName in Enum.GetValues(typeof(AbilityNames)))
            {
                UpdateUnlockedAbilityUI(abilityName);
                UpdateUpgradeAbilityUI(abilityName);
            }
        }

        private void UpdateButtonInteractability(SDAbilityData ability, int index, int currentPoints) =>
            abilityUpgraderButton[index].interactable = ability.IsUnlocked && currentPoints >= ability.UpgradeCost;
    }
}
