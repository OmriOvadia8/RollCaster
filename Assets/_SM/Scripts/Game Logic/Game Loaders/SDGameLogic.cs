using System;
using SD_Core;
using SD_Currency;

namespace SD_GameLoad
{
    public class SDGameLogic : ISDBaseManager
    {
        public static SDGameLogic Instance;

        public SDScoreManager ScoreManager;
       // public SDStoreManager StoreManager;

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
            ScoreManager = new SDScoreManager();
           // StoreManager = new SDStoreManager();

            onComplete.Invoke();
        }
    }
}