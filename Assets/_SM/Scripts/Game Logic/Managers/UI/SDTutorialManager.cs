using UnityEngine;
using SD_Core;
using System.Collections;

namespace SD_UI
{
    public class SDTutorialManager : SDMonoBehaviour
    {
        [SerializeField] GameObject ExplanationGuide;

        private void Start() => WaitForFrame(CheckIfFirstTime);

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