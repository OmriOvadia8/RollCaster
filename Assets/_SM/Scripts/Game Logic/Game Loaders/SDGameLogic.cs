using System;
using SD_Core;

namespace SD_GameLoad
{
    public class SDGameLogic : ISDBaseManager
    {
        public static SDGameLogic Instance;
        public SDScoreManager ScoreManager;
        public SDAbilityDataManager AbilityData;
        public SDBossDataManager CurrentBossData;

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
            CurrentBossData = new();
            SDDebug.Log($"GameLogic Data Initialized");
            onComplete.Invoke();
        }
    }
}
