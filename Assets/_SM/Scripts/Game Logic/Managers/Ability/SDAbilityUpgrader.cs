using SD_Core;
using SD_GameLoad;
using System;

namespace SD_Ability
{
    /// <summary>
    /// Manages the upgrading and unlocking of abilities in the game.
    /// </summary>
    public class SDAbilityUpgrader : SDLogicMonoBehaviour
    {
        private void OnEnable() => AddListener(SDEventNames.CheckUnlockAbility, CheckForAbilityUnlock);

        private void OnDisable() => RemoveListener(SDEventNames.CheckUnlockAbility, CheckForAbilityUnlock);

        /// <summary>
        /// Upgrades a given ability if the player has enough ability points.
        /// </summary>
        /// <param name="abilityName">The name of the ability to upgrade.</param>
        public void UpgradeAbility(string abilityName)
        {
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);
            
            if (ability != null)
            {
                var cost = ability.UpgradeCost;
                if (GameLogic.PlayerController.CanSpendAbilityPoints(cost))
                {
                    GameLogic.PlayerController.SpendAbilityPoints(cost);
                    ability.UpgradeAbility();
                    InvokeEvent(SDEventNames.SpendPointsToast, cost);
                    InvokeEvent(SDEventNames.UpdateAbilityUpgradeUI, Enum.Parse(typeof(AbilityNames), ability.AbilityName));
                    GameLogic.AbilityData.SaveAbilityData();
                }
                else
                {
                    SDDebug.LogException("Player does not have enough ability points to upgrade this ability or ability is null.");
                }
            }
        }

        /// <summary>
        /// Checks if an ability should be unlocked based on the current player level.
        /// </summary>
        /// <param name="playerLevel">The player's current level.</param>
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
                    InvokeEvent(SDEventNames.NewSkillToast, null);
                }
            }
        }
    }
}
