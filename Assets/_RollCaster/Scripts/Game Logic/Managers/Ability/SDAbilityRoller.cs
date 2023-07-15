using System.Linq;
using UnityEngine;
using SD_GameLoad;

namespace SD_Ability
{
    /// <summary>
    /// Handles the random selection of abilities and the outcome of dice rolls in the game.
    /// </summary>
    public class SDAbilityRoller : SDLogicMonoBehaviour
    {
        /// <summary>
        /// Retrieves a random unlocked ability from the current set of abilities.
        /// </summary>
        /// <returns>A random <see cref="SDAbilityData"/> object from the set of unlocked abilities.</returns>
        public SDAbilityData GetRandomAbility()
        {
            var abilities = SDAbilityDataManager.Abilities.AbilitiesInfo;
            var unlockedAbilities = abilities.Where(a => a.IsUnlocked).ToArray();
            int index = Random.Range(0, unlockedAbilities.Length);
            return unlockedAbilities[index];
        }

        /// <summary>
        /// Simulates a dice roll, producing a random integer outcome between 1 and 6 inclusive.
        /// </summary>
        /// <returns>An integer outcome of a dice roll.</returns>
        public int DiceOutcome()
        {
            int outcome = Random.Range(1, 7);
            return outcome;
        }
    }
}
