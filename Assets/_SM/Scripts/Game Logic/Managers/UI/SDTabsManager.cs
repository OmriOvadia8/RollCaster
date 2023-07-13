using UnityEngine.UI;
using UnityEngine;
using SD_Core;
using SD_GameLoad;

namespace SD_UI
{
    /// <summary>
    /// Handles the User Interface interactions for the tab views in the game, including managing tab visibility and interactions.
    /// </summary>
    public class TabsManager : SDLogicMonoBehaviour
    {
        [SerializeField] GameObject[] tabs;
        [SerializeField] Button[] tabButtons;

        private int currentTab = 0;

        private void Start()
        {
            ShowTab(currentTab);
        }

        /// <summary>
        /// Displays the tab indicated by the passed index and disables its corresponding button.
        /// Also hides the previously active tab and enables its button.
        /// </summary>
        /// <param name="tabIndex">Index of the tab to be displayed.</param>
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