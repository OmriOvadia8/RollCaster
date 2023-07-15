using System;
using System.Collections.Generic;
using System.IO;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using Firebase.Extensions;
using System.Threading.Tasks;

namespace SD_Core
{
    /// <summary>
    /// Manages the configuration data of the game using Firebase Remote Config and local config files.
    /// </summary>
    public class SDConfigManager
    {
        private Action onInit;

        /// <summary>
        /// Initializes a new instance of the SDConfigManager class and sets the default configuration.
        /// </summary>
        /// <param name="onComplete">The action to be executed when the initialization is complete.</param>
        public SDConfigManager(Action onComplete)
        {
            SDDebug.Log($"SDConfigManager");

            onInit = onComplete;

            var defaults = new Dictionary<string, object>();
            defaults.Add("upgrade_config", "{}");

            SDDebug.Log("SDConfigManager");
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(OnDefaultValuesSet);
        }

        private void OnDefaultValuesSet(Task task)
        {
            SDDebug.Log("OnDefaultValuesSet");

            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(OnFetchComplete);
        }

        private void OnFetchComplete(Task obj)
        {
            SDDebug.Log("OnFetchComplete");

            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task => OnActivateComplete(task));
        }

        private void OnActivateComplete(Task obj)
        {
            SDDebug.Log("OnActivateComplete");
            onInit.Invoke();
        }

        /// <summary>
        /// Fetches the requested configuration data asynchronously from Firebase Remote Config.
        /// </summary>
        /// <param name="configID">The ID of the configuration data to be fetched.</param>
        /// <param name="onComplete">The action to be executed when the configuration data is fetched.</param>
        public void GetConfigAsync<T>(string configID, Action<T> onComplete)
        {
            SDDebug.Log($"GetConfigAsync {configID}");

            var saveJson = FirebaseRemoteConfig.DefaultInstance.GetValue(configID).StringValue;

            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);
        }

        /// <summary>
        /// Fetches the requested configuration data asynchronously from a local configuration file.
        /// </summary>
        /// <param name="configID">The ID of the configuration data to be fetched.</param>
        /// <param name="onComplete">The action to be executed when the configuration data is fetched.</param>
        public void GetConfigOfflineAsync<T>(string configID, Action<T> onComplete)
        {
            var path = $"Assets/_SD/Config/{configID}.json";

            var saveJson = File.ReadAllText(path);
            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);
        }
    }
}
