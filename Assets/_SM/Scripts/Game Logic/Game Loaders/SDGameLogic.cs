using System;
using SD_Core;

namespace SD_GameLoad
{
    public class SDGameLogic : ISDBaseManager
    {
        public static SDGameLogic Instance;
        public SDScoreManager ScoreManager;
        public SDAbilityDataManager AbilityData;

        public SDGameLogic()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
        }

        public void LoadManager(Action onComplete)
        {
            ScoreManager = new();
            AbilityData = new();
            SDDebug.Log($"Ability Data Initialized");
            onComplete.Invoke();
        }
    }
}
