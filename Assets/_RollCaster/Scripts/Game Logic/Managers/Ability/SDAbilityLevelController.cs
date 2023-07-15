using SD_GameLoad;
using SD_Core;

namespace SD_Ability
{
    /// <summary>
    /// Handles the leveling up of abilities in the game, including updating the damage caused by an ability when its level increases.
    /// </summary>
    public class SDAbilityLevelController : SDLogicMonoBehaviour
    {
        private double damageIncreaseBy = 1.2d;

        /// <summary>
        /// Increases the level of the specified ability and scales its damage accordingly.
        /// </summary>
        /// <param name="abilityName">The name of the ability to increase the level of.</param>
        public void IncreaseLevel(string abilityName)
        {
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);
            if (ability != null)
            {
                ability.Level++;
                ability.Damage *= damageIncreaseBy;

                GameLogic.AbilityData.SaveAbilityData();
                SDDebug.Log($"New level: {ability.Level}, New damage: {ability.Damage}");
            }
            else
            {
                SDDebug.LogException("Ability not found: " + abilityName);
            }
        }
    }
}