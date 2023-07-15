using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace SD_Core
{
    public class InitializeUnityServices : SDMonoBehaviour
    {
        public string environment = "production";

        async void Awake()
        {
            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName(environment);

                await UnityServices.InitializeAsync(options);
            }
            catch (Exception exception)
            {
                SDDebug.LogException(exception);
                Manager.AnalyticsManager.ReportEvent(SDEventType.unity_services_failed);
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}