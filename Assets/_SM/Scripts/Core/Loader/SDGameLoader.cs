using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SD_Core
{
    /// <summary>
    /// Manages the loading and initialization of the game.
    /// </summary>
    public class SDGameLoader : SDMonoBehaviour
    {
        [SerializeField] private SDGameLoaderBase gameLogicLoader;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(DelayStartCoroutine());
        }

        /// <summary>
        /// Starts the game initialization process with a delay.
        /// </summary>
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
