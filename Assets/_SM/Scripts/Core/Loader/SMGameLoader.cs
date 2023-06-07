using UnityEngine;
using UnityEngine.SceneManagement;

namespace SM_Core
{
    public class SMGameLoader : SMMonoBehaviour
    {
        [SerializeField] private SMGameLoaderBase gameLogicLoader;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            WaitForFrame(DelayStart);
        }

        private void DelayStart()
        {
            var manager = new SMManager();

            manager.LoadManager(() =>
            {
                gameLogicLoader.StartLoad(() =>
                {
                    SceneManager.LoadScene(1);
                    Manager.AnalyticsManager.ReportEvent(SMEventType.app_loaded);
                    ShowMessage();
                });
            });
        }

        private void ShowMessage()
        {
            int offlineTime = Manager.TimerManager.GetLastOfflineTimeSeconds();

            WaitForFrame(() =>
            {
                if(offlineTime == 0)
                {
                    Manager.PopupManager.AddPopupToQueue(SMPopupData.FirstLoginMessage);
                }

                else
                {
                    Manager.PopupManager.AddPopupToQueue(SMPopupData.WelcomeBackMessage);
                }

                Destroy(gameObject);
            });
        }
    }
}