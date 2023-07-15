using System;
using SD_Core;

namespace SD_GameLoad
{
    /// <summary>
    /// Manages game logic, including ability data, boss data, player data and the store.
    /// </summary>
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

        /// <summary>
        /// Initializes the game logic manager.
        /// </summary>
        /// <param name="onComplete">A callback function to be invoked when initialization is complete.</param>
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