using UnityEngine;
using SD_GameLoad;

namespace SD_Ability
{
    public class SDAbilityAnimationController : SDLogicMonoBehaviour
    {
        [SerializeField] private Animator animator;
        private readonly int firstAnimLevel = 10;
        private readonly int secondAnimLevel = 20;

        public void UseAbility(string abilityName)
        {
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName);
            if (ability != null)
            {
                string animationName = DetermineAnimation(abilityName, ability.Level);
                animator.SetTrigger(animationName);
            }
        }

        private string DetermineAnimation(string abilityName, int level)
        {
            int animationSuffix;

            if (level <= firstAnimLevel)
            {
                animationSuffix = 1;
            }
            else if (level <= secondAnimLevel)
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

    public enum AbilityNames
    {
        SkullSmoke = 0,
        Slashes = 1,

    }
}