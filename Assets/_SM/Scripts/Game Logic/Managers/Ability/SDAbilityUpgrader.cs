using SD_Core;
using SD_GameLoad;
using System;

namespace SD_Ability
{
    public class SDAbilityUpgrader : SDLogicMonoBehaviour
    {
        private void OnEnable() => AddListener(SDEventNames.CheckUnlockAbility, CheckForAbilityUnlock);

        private void OnDisable() => RemoveListener(SDEventNames.CheckUnlockAbility, CheckForAbilityUnlock);
        public void UpgradeAbility(string abilityName)
        {
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);

            if (ability != null)
            {
                if (GameLogic.PlayerController.CanSpendAbilityPoints(ability.UpgradeCost))
                {
                    ability.UpgradeAbility();
                    GameLogic.PlayerController.SpendAbilityPoints(ability.UpgradeCost);
                    InvokeEvent(SDEventNames.UpdateAbilityUpgradeUI, Enum.Parse(typeof(AbilityNames), ability.AbilityName));
                    GameLogic.AbilityData.SaveAbilityData();
                    SDDebug.Log($"{ability.Level} in {ability.AbilityName} with {ability.Damage}");
                }
                else
                {
                    SDDebug.Log("Player does not have enough ability points to upgrade this ability.");
                }
            }
        }


        private void CheckForAbilityUnlock(object playerLevel)
        {
            int level = (int)playerLevel;
            foreach (AbilityNames abilityName in Enum.GetValues(typeof(AbilityNames)))
            {
                var ability = GameLogic.AbilityData.FindAbilityByName(abilityName.ToString());

                if (ability == null)
                {
                    SDDebug.LogException(ability + "is null");
                    continue;
                }

                if (!ability.IsUnlocked && level >= ability.UnlockLevel)
                {
                    ability.UnlockAbility();
                    GameLogic.AbilityData.SaveAbilityData();
                    SDDebug.Log($"{ability.AbilityName} is unlocked");

                    InvokeEvent(SDEventNames.UpdateAbilityUnlockedUI, abilityName);
                }
            }
        }

    }
}
