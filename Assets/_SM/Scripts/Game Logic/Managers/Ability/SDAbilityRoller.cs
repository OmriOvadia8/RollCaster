using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;

namespace SD_Ability
{
    public class SDAbillityRoller : SerializedMonoBehaviour
    {
        [SerializeField] List<Sprite> abilityIcons; // Holds the current level sprites of all abilities
        [SerializeField] Image abilityImage; // The image component on your ability button

        public void StartRolling()
        {
            StartCoroutine(RollingRoutine());
        }

        private IEnumerator RollingRoutine()
        {
            float rollTime = 1f; // Duration of the roll in seconds
            float timer = 0f;

            while (timer < rollTime)
            {
                int index = Random.Range(0, abilityIcons.Count);
                abilityImage.sprite = abilityIcons[index];
                yield return null;
                timer += Time.deltaTime;
            }

            // Lock in the final ability
            int finalIndex = Random.Range(0, abilityIcons.Count);
            abilityImage.sprite = abilityIcons[finalIndex];
        }

        public void ChangeIconInList(Sprite oldAbilitySprite, Sprite newAbilitySprite)
        {
            abilityIcons.Remove(oldAbilitySprite);
            abilityIcons.Add(newAbilitySprite);
        }
    }
}
