using UnityEngine;
using SD_Core;

namespace SD_UI
{
    /// <summary>
    /// Manages the tutorial or game guide functionality in the game.
    /// </summary>
    public class SDTutorialManager : SDMonoBehaviour
    {
        [SerializeField] GameObject ExplanationGuide;

        private void Start() => WaitForFrame(CheckIfFirstTime);

        /// <summary>
        /// Checks if it's the player's first time playing the game (last offline time is zero). 
        /// If it is, it opens the game explanation guide.
        /// </summary>
        private void CheckIfFirstTime()
        {
            if (Manager.TimerManager.GetLastOfflineTimeSeconds() == 0)
            {
                GameExplanationOpener(true);
            }
        }

        public void GameExplanationOpener(bool value) => ExplanationGuide.SetActive(value);
    }
}