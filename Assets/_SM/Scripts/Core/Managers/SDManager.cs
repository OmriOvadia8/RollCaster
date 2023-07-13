using System;
using Firebase.Extensions;

namespace SD_Core
{
    /// <summary>
    /// The Singleton Manager class that initializes and manages all core managers in the game.
    /// </summary>
    public class SDManager : ISDBaseManager
    {
        public static SDManager Instance;

        public SDEventsManager EventsManager;
        public SDFactoryManager FactoryManager;
        public SDPoolManager PoolManager;
        public SDSaveManager SaveManager;
        public SDConfigManager ConfigManager;
        public SDCrashManager CrashManager;
        public SDAnalyticsManager AnalyticsManager;
        public SDTimeManager TimerManager;
        public SDMonoManager MonoManager;
        public SDInAppPurchase PurchaseManager;
        public SDAdsManager AdsManager;

        public Action onInitAction;

        public SDManager()
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

        /// <summary>
        /// Initializes the Firebase App and fixes dependencies if they are available.
        /// If successful, invokes the passed action.
        /// </summary>
        public void InitFirebase(Action onComplete)
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    var app = Firebase.FirebaseApp.DefaultInstance;
                    SDDebug.Log($"Firebase was initialized");
                    onComplete.Invoke();
                }
                else
                {
                    SDDebug.LogException($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }

        /// <summary>
        /// Initializes all core manager classes.
        /// Invokes the onInitAction after successful initialization of all managers.
        /// </summary>
        private void InitManagers()
        {
            MonoManager = new SDMonoManager();
            CrashManager = new SDCrashManager();

            ConfigManager = new SDConfigManager(delegate
            {
            EventsManager = new SDEventsManager();
            AnalyticsManager = new SDAnalyticsManager();
            FactoryManager = new SDFactoryManager();
            PoolManager = new SDPoolManager();
            AdsManager = new SDAdsManager();
            SaveManager = new SDSaveManager();
            PurchaseManager = new SDInAppPurchase();
            TimerManager = new SDTimeManager();
            SDDebug.Log($"After all managers loaded");
            onInitAction.Invoke();
         });
        }
    }
}
