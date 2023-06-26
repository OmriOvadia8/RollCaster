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
        public SDBossController BossController;
        public SDPlayerDataManager Player;
        public SDPlayerController PlayerController;

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
            BossController = new();
            Player = new();
            PlayerController = new();

            SDDebug.Log($"GameLogic Data Initialized");
            onComplete.Invoke();
        }
    }
}

