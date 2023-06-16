using SD_Ability;
using UnityEngine;
using SD_GameLoad;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace SD_UI
{
    public class SDAbilityRollingUI : SDLogicMonoBehaviour
    {
        [SerializeField] SDAbilityRoller abilityRoller;
        [SerializeField] Image chosenAbilityIcon;
        [SerializeField] TMP_Text diceText;
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
                diceText.text = abilityRoller.DiceOutcome().ToString();
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            int diceOutcome = abilityRoller.DiceOutcome();
            diceText.text = diceOutcome.ToString();
            SDAbilityData chosenAbility = abilityRoller.GetRandomAbility();
            ChosenAbilityIconChange(chosenAbility);
            abilityAnimationController.UseAbility(chosenAbility.AbilityName, diceOutcome);
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