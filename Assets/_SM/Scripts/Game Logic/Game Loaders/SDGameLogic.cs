using System;
using SD_Core;

namespace SD_GameLoad
{
    public class SDGameLogic : ISDBaseManager
    {
        public static SDGameLogic Instance;
        public SDAbilityDataManager AbilityData;
        public SDBossDataManager CurrentBossData;
        public SDBossController BossController;
        public SDPlayerDataManager Player;
        public SDPlayerController PlayerController;
        public SDStoreManager StoreManager;

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
            AbilityData = new();
            CurrentBossData = new();
            BossController = new();
            Player = new();
            PlayerController = new();
            StoreManager = new();

            SDDebug.Log("GameLogic Data Initialized");
            onComplete.Invoke();
        }
    }
}