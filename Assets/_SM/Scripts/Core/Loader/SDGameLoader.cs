using UnityEngine;
using UnityEngine.SceneManagement;

namespace SD_Core
{
    public class SDGameLoader : SDMonoBehaviour
    {
        [SerializeField] private SDGameLoaderBase gameLogicLoader;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            WaitForFrame(DelayStart);
        }

        private void DelayStart()
        {
            var manager = new SDManager();

            manager.LoadManager(() =>
            {
                gameLogicLoader.StartLoad(() =>
                {
                    SceneManager.LoadScene(1);
                    Manager.AnalyticsManager.ReportEvent(SDEventType.app_loaded);
                });
            });
        }
    }
}