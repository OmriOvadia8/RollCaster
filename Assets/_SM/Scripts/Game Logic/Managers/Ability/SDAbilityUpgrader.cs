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
            var playerInfo = GameLogic.Player.PlayerData.PlayerInfo;
            int currentPoints = playerInfo.AbilityPoints;
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);

            if (ability != null && currentPoints >= ability.UpgradeCost)
            {
                playerInfo.AbilityPoints -= ability.UpgradeCost;
                ability.UpgradeAbility();
                InvokeEvent(SDEventNames.UpdateAbilityPtsUI, null);
                InvokeEvent(SDEventNames.UpdateAbilityUpgradeUI, Enum.Parse(typeof(AbilityNames), ability.AbilityName));
                GameLogic.Player.SavePlayerData();
                GameLogic.AbilityData.SaveAbilityData();
                SDDebug.Log($"{ability.Level} in {ability.AbilityName} with {ability.Damage}");
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
