using System;
using Firebase.Extensions;

namespace SM_Core
{
    public class SMManager : ISMBaseManager
    {
        public static SMManager Instance;

        public SMEventsManager EventsManager;
        public SMFactoryManager FactoryManager;
        public SMPoolManager PoolManager;
        public SMSaveManager SaveManager;
        public SMConfigManager ConfigManager;
        public SMCrashManager CrashManager;
        public SMAnalyticsManager AnalyticsManager;
        public SMTimeManager TimerManager;
        public SMMonoManager MonoManager;
        // public SMInAppPurchase PurchaseManager;
        // public SMAdsManager AdsManager;
        //public SMPopupManager PopupManager;

        public Action onInitAction;

        public SMManager()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
        }

        public void LoadManager(Action onComplete)
        {
            onInitAction = onComplete;
            InitFirebase(delegate
            {
                InitManagers();
            });
        }

        public void InitFirebase(Action onComplete)
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    var app = Firebase.FirebaseApp.DefaultInstance;
                    SMDebug.Log($"Firebase was initialized");
                    onComplete.Invoke();
                }
                else
                {
                    SMDebug.LogException($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }

        private void InitManagers()
        {
            MonoManager = new SMMonoManager();
            SMDebug.Log($"InitManagers");

            CrashManager = new SMCrashManager();
            SMDebug.Log($"After CrashManager");

            ConfigManager = new SMConfigManager(delegate
            {
            EventsManager = new SMEventsManager();
            SMDebug.Log($"After SMEventsManager");

            AnalyticsManager = new SMAnalyticsManager();

            FactoryManager = new SMFactoryManager();
            SMDebug.Log($"After SMFactoryManager");

            PoolManager = new SMPoolManager();
            SMDebug.Log($"After SMPoolManager");

            SaveManager = new SMSaveManager();
            SMDebug.Log($"After SMSaveManager");

            SMDebug.Log($"Before Config Manager");

            TimerManager = new SMTimeManager();

            // PurchaseManager = new SMInAppPurchase();

            // AdsManager = new SMAdsManager();

            //PopupManager = new SMPopupManager();
                SMDebug.Log($"After all managers loaded");
                onInitAction.Invoke();
            
         });
        }
    }
}
