using System;
using SM_Core;
using SM_GameManagers;

namespace SM_GameLoad
{
    public class SMGameLogic : ISMBaseManager
    {
        public static SMGameLogic Instance;

        public SMScoreManager ScoreManager;
       // public SMStoreManager StoreManager;

        public SMGameLogic()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
        }

        public void LoadManager(Action onComplete)
        {
            ScoreManager = new SMScoreManager();
           // StoreManager = new SMStoreManager();

            onComplete.Invoke();
        }
    }
}