using SD_Ability;
using UnityEngine;
using SD_GameLoad;
using UnityEngine.UI;
using System.Collections;
using SD_Core;

namespace SD_UI
{
    public class SDAbilityRollingUI : SDLogicMonoBehaviour
    {
        [SerializeField] SDAbilityRoller abilityRoller;
        [SerializeField] Image chosenAbilityIcon;
        [SerializeField] Image diceImage;
        [SerializeField] Sprite[] diceResultSprite;
        [SerializeField] SDAbilityAnimationController abilityAnimationController;
        [SerializeField] Button spinButton;

        private void OnEnable()
        {
            AddListener(SDEventNames.SpinEnable, SpinEnable);
            AddListener(SDEventNames.CheckRollsForSpin, CheckRollsForSpin);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.SpinEnable, SpinEnable);
            RemoveListener(SDEventNames.CheckRollsForSpin, CheckRollsForSpin);
        }

        private void Start() => CheckRollsForSpin();

        public void OnRoll() => StartCoroutine(RollCoroutine());

        private IEnumerator RollCoroutine()
        {
            float rollDuration = 1.75f;
            float elapsedTime = 0;
            float waitTime = 0.075f;

            while (elapsedTime < rollDuration)
            {
                SpinEnable(false);
                SDAbilityData randomAbility = abilityRoller.GetRandomAbility();
                ChosenAbilityIconChange(randomAbility);
                int diceRolling = abilityRoller.DiceOutcome();
                diceImage.sprite = diceResultSprite[diceRolling - 1];
                yield return new WaitForSeconds(waitTime);
                elapsedTime += waitTime;
            }

            int diceOutcome = abilityRoller.DiceOutcome();
            diceImage.sprite = diceResultSprite[diceOutcome - 1];
            SDAbilityData chosenAbility = abilityRoller.GetRandomAbility();
            ChosenAbilityIconChange(chosenAbility);
            abilityAnimationController.UseAbility(chosenAbility.AbilityName, diceOutcome);
            CheckRollsForSpin();
        }

        private void ChosenAbilityIconChange(SDAbilityData abilityData)
        {
            Sprite abilityIcon = abilityData.GetIcon();
            chosenAbilityIcon.sprite = abilityIcon;
        }

        private void CheckRollsForSpin(object obj = null)
        {
            SpinEnable(false);

            if (GameLogic.Player.PlayerData.PlayerInfo.CurrentRolls > 0)
            {
                SpinEnable(true);
            }
        }

        private void SpinEnable(object boolValue)
        {
            bool value = (bool)boolValue;
            spinButton.interactable = value;
        }
    }
}