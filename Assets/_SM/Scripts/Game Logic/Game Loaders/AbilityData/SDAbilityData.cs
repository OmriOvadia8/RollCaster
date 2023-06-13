using SD_Core;
using System;

namespace SD_GameLoad
{
    [Serializable]
    public class SDAbilityData
    {
        public string AbilityName { get; set; }
        public int Level { get; set; }
        public bool IsUnlocked { get; set; }

        public SDAbilityData(string abilityName, int level, bool isUnlocked)
        {
            AbilityName = abilityName;
            Level = level;
            IsUnlocked = isUnlocked;
        }
    }

    [Serializable]
    public class AbilityDataCollection : ISDSaveData
    {
        public SDAbilityData[] AbilitiesInfo { get; set; }
    }
}