using UnityEngine;

namespace SD_UI
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData")]
    public class AbilityUIDataSO : ScriptableObject
    {
        public string abilityName;
        public Sprite abilityIcon;
    }
}