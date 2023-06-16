using System.Linq;
using UnityEngine;
using SD_GameLoad;

namespace SD_Ability
{
    public class SDAbilityRoller : SDLogicMonoBehaviour
    {
        public SDAbilityData GetRandomAbility()
        {
            var abilities = SDAbilityDataManager.Abilities.AbilitiesInfo;
            var unlockedAbilities = abilities.Where(a => a.IsUnlocked).ToArray();
            int index = Random.Range(0, unlockedAbilities.Length);
            return unlockedAbilities[index];
        }

        public int DiceOutcome()
        {
            int outcome = Random.Range(1, 7);
            return outcome;
        }
    }
}
