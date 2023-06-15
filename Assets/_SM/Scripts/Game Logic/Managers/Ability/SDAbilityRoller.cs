using System.Linq;
using UnityEngine;
using SD_GameLoad;

namespace SD_Ability
{
    public class SDAbilityRoller : SDLogicMonoBehaviour
    {
        public SDAbilityData RollSkill()
        {
            var abilities = SDAbilityDataManager.Abilities.AbilitiesInfo;

            var unlockedAbilities = abilities.Where(a => a.IsUnlocked).ToArray();

            int index = Random.Range(0, unlockedAbilities.Length);

            return unlockedAbilities[index];
        }
    }
}
