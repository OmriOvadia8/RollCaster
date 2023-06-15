using SD_Ability;
using UnityEngine;
using SD_GameLoad;
using UnityEngine.UI;
using System.Collections;

namespace SD_UI
{
    public class SDAbilityRollingUI : SDLogicMonoBehaviour
    {
        [SerializeField] SDAbilityRoller abilityRoller;
        [SerializeField] Image chosenAbilityIcon;
        [SerializeField] SDAbilityAnimationController abilityAnimationController;
        [SerializeField] Button spinButton;

        public void OnRoll() => StartCoroutine(RollCoroutine());

        private IEnumerator RollCoroutine()
        {
            float rollDuration = 1.5f;
            float elapsedTime = 0.0f;

            while (elapsedTime < rollDuration)
            {
                SpinEnable(false);
                SDAbilityData randomAbility = abilityRoller.GetRandomAbility();
                ChosenAbilityIconChange(randomAbility);
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            SDAbilityData chosenAbility = abilityRoller.GetRandomAbility();
            ChosenAbilityIconChange(chosenAbility);
            abilityAnimationController.UseAbility(chosenAbility.AbilityName);
            SpinEnable(true);
        }

        private void ChosenAbilityIconChange(SDAbilityData abilityData)
        {
            Sprite abilityIcon = abilityData.GetIcon();
            chosenAbilityIcon.sprite = abilityIcon;
        }

        private void SpinEnable(bool value) => spinButton.interactable = value;
    }
}