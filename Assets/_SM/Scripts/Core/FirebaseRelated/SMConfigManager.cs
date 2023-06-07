using System;
using System.Collections.Generic;
using System.IO;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using Firebase.Extensions;
using System.Threading.Tasks;

namespace SM_Core
{
    public class SMConfigManager
    {
        private Action onInit;

        public SMConfigManager(Action onComplete)
        {
            SMDebug.Log($"SMConfigManager");

            onInit = onComplete;

            var defaults = new Dictionary<string, object>();
            defaults.Add("upgrade_config", "{}");

            SMDebug.Log("SMConfigManager");
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(OnDefaultValuesSet);
        }

        private void OnDefaultValuesSet(Task task)
        {
            SMDebug.Log("OnDefaultValuesSet");

            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(OnFetchComplete);
        }

        private void OnFetchComplete(Task obj)
        {
            SMDebug.Log("OnFetchComplete");

            FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task => OnActivateComplete(task));
        }

        private void OnActivateComplete(Task obj)
        {
            SMDebug.Log("OnActivateComplete");
            onInit.Invoke();
        }

        public void GetConfigAsync<T>(string configID, Action<T> onComplete)
        {
            SMDebug.Log($"GetConfigAsync {configID}");

            var saveJson = FirebaseRemoteConfig.DefaultInstance.GetValue(configID).StringValue;

            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);
        }

        public void GetConfigOfflineAsync<T>(string configID, Action<T> onComplete)
        {
            var path = $"Assets/_SM/Config/{configID}.json";

            var saveJson = File.ReadAllText(path);
            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);
        }
    }
}