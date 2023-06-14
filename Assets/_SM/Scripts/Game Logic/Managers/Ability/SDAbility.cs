using SD_Core;
using UnityEngine;
using UnityEngine.UI;

namespace SD_Ability
{
    [CreateAssetMenu(menuName = "Ability")]
    public class SDAbility : ScriptableObject
    {
        public string abilityName;
        public AbilityAnimationNames[] versions;
        public Sprite[] levelIcons;
    }
}
