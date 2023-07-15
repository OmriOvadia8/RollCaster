using UnityEngine;
using SD_GameLoad;
using SD_Core;

namespace SD_Ability
{
    /// <summary>
    /// Handles the animations for abilities in the game based on the ability's name and a dice outcome.
    /// </summary>
    public class SDAbilityAnimationController : SDLogicMonoBehaviour
    {
        [SerializeField] private Animator animator;
        private readonly int firstAnimLevel = 3;
        private readonly int secondAnimLevel = 5;

        /// <summary>
        /// Executes the appropriate animation for the given ability and dice outcome. 
        /// Decreases the player's roll count after the animation is set.
        /// </summary>
        /// <param name="abilityName">The name of the ability to animate.</param>
        /// <param name="diceOutcome">The result of a dice roll which determines the specific animation variant to be played.</param>
        public void UseAbility(string abilityName, int diceOutcome)
        {
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);
            if (ability != null)
            {
                string animationName = DetermineAnimation(abilityName, diceOutcome);
                animator.SetTrigger(animationName);
                GameLogic.PlayerController.DecreaseRoll();
                InvokeEvent(SDEventNames.CheckRollsForSpin, null);
            }
        }

        /// <summary>
        /// Determines the correct animation to be played based on the ability's name and a dice outcome.
        /// </summary>
        /// <param name="abilityName">The name of the ability to animate.</param>
        /// <param name="diceOutcome">The result of a dice roll which determines the specific animation variant.</param>
        /// <returns>The name of the animation to be played.</returns>
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