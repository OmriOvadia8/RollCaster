using SD_Core;
using System;
using UnityEngine;

namespace SD_GameLoad
{
    [Serializable]
    public class SDAbilityData
    {
        public string AbilityName { get; set; }
        public int Level { get; set; }
        public double Damage { get; set; }
        public bool IsUnlocked { get; set; }
        public string IconResourcePath { get; set; }

        public SDAbilityData(string abilityName, int level, double damage, bool isUnlocked, string iconResourcePath)
        {
            AbilityName = abilityName;
            Level = level;
            Damage = damage;
            IsUnlocked = isUnlocked;
            IconResourcePath = iconResourcePath;
        }

        public Sprite GetIcon()
        {
            return Resources.Load<Sprite>(IconResourcePath);
        }

    }

    [Serializable]
    public class AbilityDataCollection : ISDSaveData
    {
        public SDAbilityData[] AbilitiesInfo { get; set; }
    }

    [Serializable]
    public enum AbilityNames
    {
        SkullSmoke = 0,
        Slashes = 1,
        SmokeExplosion = 2,
        SkullExplosion = 3,
        Scratch = 4,
        Tornado = 5,
        Tentacle = 6,
    }
}
