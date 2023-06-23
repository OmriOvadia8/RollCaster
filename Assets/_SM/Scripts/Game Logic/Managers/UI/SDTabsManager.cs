using UnityEngine.UI;
using UnityEngine;
using SD_Core;
using SD_GameLoad;

namespace SD_UI
{
    public class TabsManager : SDLogicMonoBehaviour
    {
        [SerializeField] GameObject[] tabs;
        [SerializeField] Button[] tabButtons;

        private int currentTab = 0;

        private void Start()
        {
            ShowTab(currentTab);
        }

        public void ShowTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= tabs.Length)
            {
                SDDebug.LogException($"Invalid tab index: {tabIndex}");
                return;
            }

            tabs[currentTab].SetActive(false);
            tabButtons[currentTab].interactable = true;

            currentTab = tabIndex;

            tabs[currentTab].SetActive(true);
            tabButtons[currentTab].interactable = false;
        }
    }
}