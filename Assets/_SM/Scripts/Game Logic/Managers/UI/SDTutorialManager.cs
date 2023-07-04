using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD_UI
{
    public class SDTutorialManager : MonoBehaviour
    {
        [SerializeField] GameObject ExplanationGuide;

        public void WindowsOpener(bool value)
        {
            ExplanationGuide.SetActive(value);
        }
    }
}