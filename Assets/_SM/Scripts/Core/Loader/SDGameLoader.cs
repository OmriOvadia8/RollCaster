using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SD_Core
{
    public class SDGameLoader : SDMonoBehaviour
    {
        [SerializeField] private SDGameLoaderBase gameLogicLoader;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(DelayStartCoroutine());
        }

        private IEnumerator DelayStartCoroutine()
        {
            yield return new WaitForSeconds(1);
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
