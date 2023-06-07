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
            Debug.Log("DelayStart called");
            var manager = new SMManager();

            manager.LoadManager(() =>
            {
                Debug.Log("Inside LoadManager callback");
                gameLogicLoader.StartLoad(() =>
                {
                    Debug.Log("Inside StartLoad callback");
                    SceneManager.LoadScene(1);
                    Debug.Log("Attempted to load scene 1");
                    Manager.AnalyticsManager.ReportEvent(SMEventType.app_loaded);
                });
            });
        }
    }
}