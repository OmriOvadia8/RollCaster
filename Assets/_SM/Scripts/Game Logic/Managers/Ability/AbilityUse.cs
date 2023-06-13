using UnityEngine;
using SD_Core;
using System;
using SD_GameLoad;

namespace SD_Ability
{
    public class AbilityUse : SDLogicMonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void UseAbility(string abilityName)
        {
            var ability = Array.Find(SDAbilityDataManager.Abilities.AbilitiesInfo, a => a.AbilityName == abilityName);
            if (ability != null)
            {
                string animationName = DetermineAnimation(abilityName, ability.Level);
                animator.SetTrigger(animationName);
            }
        }

        private string DetermineAnimation(string abilityName, int level)
        {
            int animationSuffix;

            if (level <= 10)
            {
                animationSuffix = 1;
            }
            else if (level <= 20)
            {
                animationSuffix = 2;
            }
            else
            {
                animationSuffix = 3;
            }

            return abilityName + animationSuffix;
        }

        public void IncreaseLevel(string abilityName)
        {
            var ability = Array.Find(SDAbilityDataManager.Abilities.AbilitiesInfo, a => a.AbilityName == abilityName);
            if (ability != null)
            {
                ability.Level++;
                GameLogic.AbilityData.SaveAbilityData();
                SDDebug.Log(ability.Level);
            }
            else
            {
                Debug.LogError("Ability not found: " + abilityName);
            }
        }
    }
}
