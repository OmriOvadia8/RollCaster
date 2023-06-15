using SD_Ability;
using UnityEngine;
using SD_GameLoad;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace SD_UI
{
    public class SDAbilityRollingUI : SDLogicMonoBehaviour
    {
        [SerializeField] SDAbilityRoller abilityRoller;
        [SerializeField] Image chosenAbilityIcon;

        public void OnRoll()
        {
            SDAbilityData chosenAbility = abilityRoller.RollSkill();
            ChosenAbilityIconChange(chosenAbility);
        }

        private void ChosenAbilityIconChange(SDAbilityData chosenAbility)
        {
            Sprite chosenAbilityIconSprite = chosenAbility.GetIcon();
            chosenAbilityIcon.sprite = chosenAbilityIconSprite;
        }

    }
}