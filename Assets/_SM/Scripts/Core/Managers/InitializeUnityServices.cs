//using System;
//using Unity.Services.Core;
//using Unity.Services.Core.Environments;

//namespace SM_Core
//{
//    public class InitializeUnityServices : SMMonoBehaviour
//    {
//        public string environment = "production";

//        async void Awake()
//        {
//            try
//            {
//                var options = new InitializationOptions()
//                    .SetEnvironmentName(environment);

//                await UnityServices.InitializeAsync(options);
//            }
//            catch (Exception exception)
//            {
//                SMDebug.LogException(exception);
//                Manager.AnalyticsManager.ReportEvent(SMEventType.unity_services_failed);
//            }
//        }
//        private void Start()
//        {
//            DontDestroyOnLoad(gameObject);
//        }
//    }
//}