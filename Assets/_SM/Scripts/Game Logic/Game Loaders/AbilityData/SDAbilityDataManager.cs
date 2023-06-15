using SD_Core;
using System;
using UnityEngine;

namespace SD_GameLoad
{
    public class SDAbilityDataManager
    {
        public const string ABILITIES_CONFIG = "abilities";
        public static AbilityDataCollection Abilities;

        public SDAbilityDataManager() => LoadAbilitiesData();

        #region Data Loading
        private void LoadAbilitiesData()
        {
            SDManager.Instance.SaveManager.Load<AbilityDataCollection>(data =>
            {
                if (data != null)
                {
                    LoadSavedAbilityData(data);
                    SDDebug.Log("Ability data loaded from save file");
                }
                else
                {
                    LoadDefaultAbilityData();
                    SDDebug.Log("Ability data loaded from default file");
                }
            });
        }

        private void LoadSavedAbilityData(AbilityDataCollection data)
        {
            Abilities = data;
            if (Abilities != null && Abilities.AbilitiesInfo != null)
            {
                SDDebug.Log("Ability data loaded successfully. Number of abilities: " + Abilities.AbilitiesInfo.Length);
            }
            else
            {
                SDDebug.LogException("Abilities or AbilitiesInfo is null");
            }

        }

        private void OnConfigLoaded(AbilityDataCollection configData)
        {
            Abilities = configData;
            if (Abilities != null && Abilities.AbilitiesInfo != null)
            {
                SDDebug.Log("Ability data loaded successfully. Number of abilities: " + Abilities.AbilitiesInfo.Length);
            }
            else
            {
                SDDebug.LogException("Abilities or AbilitiesInfo is null");
            }

        }

        private void LoadDefaultAbilityData()
        {
            SDManager.Instance.ConfigManager.GetConfigAsync<AbilityDataCollection>(ABILITIES_CONFIG, OnConfigLoaded);
            SDDebug.Log("Default ability Data Loaded Successfully");
        }

        public void SaveAbilityData() => SDManager.Instance.SaveManager.Save(Abilities);
        #endregion

        public SDAbilityData FindAbilityByName(string abilityName)
        {
            return Array.Find(Abilities.AbilitiesInfo, a => a.AbilityName == abilityName);
        }
    }
}