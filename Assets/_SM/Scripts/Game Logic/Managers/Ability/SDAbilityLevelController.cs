using SD_GameLoad;
using SD_Core;

namespace SD_Ability
{
    public class SDAbilityLevelController : SDLogicMonoBehaviour
    {
        private double damageIncreaseBy = 1.2d;

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