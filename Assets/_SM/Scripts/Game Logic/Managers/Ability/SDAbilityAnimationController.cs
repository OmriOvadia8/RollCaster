using UnityEngine;
using SD_GameLoad;
using SD_Core;

namespace SD_Ability
{
    public class SDAbilityAnimationController : SDLogicMonoBehaviour
    {
        [SerializeField] private Animator animator;
        private readonly int firstAnimLevel = 3;
        private readonly int secondAnimLevel = 5;

        public void UseAbility(string abilityName, int diceOutcome)
        {
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);
            if (ability != null)
            {
                string animationName = DetermineAnimation(abilityName, diceOutcome);
                animator.SetTrigger(animationName);
            }
        }

        private string DetermineAnimation(string abilityName, int diceOutcome)
        {
            int animationSuffix;

            if (diceOutcome <= firstAnimLevel)
            {
                animationSuffix = 1;
            }
            else if (diceOutcome <= secondAnimLevel)
            {
                animationSuffix = 2;
            }
            else
            {
                animationSuffix = 3;
            }

            return abilityName + animationSuffix;
        }
    }
}