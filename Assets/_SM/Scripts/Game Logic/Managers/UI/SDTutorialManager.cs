using UnityEngine;
using SD_Core;

namespace SD_UI
{
    public class SDTutorialManager : SDMonoBehaviour
    {
        [SerializeField] GameObject ExplanationGuide;

        public void WindowsOpener(bool value)
        {
            ExplanationGuide.SetActive(value);
        }
    }
}