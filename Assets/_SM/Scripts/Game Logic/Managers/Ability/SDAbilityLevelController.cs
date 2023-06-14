using SD_GameLoad;
using SD_Core;

namespace SD_Ability
{
    public class SDAbilityLevelController : SDLogicMonoBehaviour
    {
        public void IncreaseLevel(string abilityName)
        {
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);
            if (ability != null)
            {
                ability.Level++;
                GameLogic.AbilityData.SaveAbilityData();
                SDDebug.Log(ability.Level);
            }
            else
            {
                SDDebug.LogException("Ability not found: " + abilityName);
            }
        }
    }
}