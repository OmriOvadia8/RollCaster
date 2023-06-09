using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;

namespace SD_Core
{
    public class SDPopupManager
    {
        public List<SDPopupData> PopupsData = new();
        public Canvas popupsCanvas;

        private Dictionary<PopupTypes, SDPopupComponentBase> cachedPopups = new();

        public SDPopupManager() => CreateCanvas();

        private void CreateCanvas()
        {
            SDManager.Instance.FactoryManager.CreateAsync("PopupCanvas", Vector3.zero, (Canvas canvas) =>
            {
                popupsCanvas = canvas;
                Object.DontDestroyOnLoad(popupsCanvas);
            });
        }

        public void AddPopupToQueue(SDPopupData popupData)
        {
            PopupsData.Add(popupData);
            TryShowNextPopup();
        }

        public void TryShowNextPopup()
        {
            if (PopupsData.Count <= 0)
            {
                return;
            }

            SortPopups();
            OpenPopup(PopupsData[0]);
        }

        public void SortPopups() => PopupsData = PopupsData.OrderBy(x => x.Priority).ToList();

        public void OpenPopup(SDPopupData dbPopupData)
        {
            dbPopupData.OnPopupClose += OnClosePopup;
            PopupsData.Remove(dbPopupData);

            if (cachedPopups.ContainsKey(dbPopupData.PopupType))
            {
                var pop = cachedPopups[dbPopupData.PopupType];
                pop.gameObject.SetActive(true);
                pop.Init(dbPopupData);
            }
            else
            {
                SDManager.Instance.FactoryManager.CreateAsync(dbPopupData.PopupType.ToString(),
                    Vector3.zero, (SDPopupComponentBase popupComponent) =>
                    {
                        cachedPopups.Add(dbPopupData.PopupType, popupComponent);
                        popupComponent.transform.SetParent(popupsCanvas.transform, false);
                        popupComponent.Init(dbPopupData);
                    });
            }
        }

        private void OnClosePopup(SDPopupComponentBase dbPopupComponentBase)
        {
            dbPopupComponentBase.gameObject.SetActive(false);
            TryShowNextPopup();
        }

        public void ClosePopup()
        {
            if (cachedPopups.Count > 0)
            {
                var lastPopup = cachedPopups.Values.LastOrDefault(popup => popup.gameObject.activeSelf);
                lastPopup?.ClosePopup();
            }
        }
        public bool IsPopupActive(SDPopupData popupData)
        {
            if (cachedPopups.TryGetValue(popupData.PopupType, out SDPopupComponentBase popupComponent))
            {
                return popupComponent.gameObject.activeSelf;
            }

            return false;
        }
    }

    public class SDPopupData
    {
        public int Priority;
        public PopupTypes PopupType;
        public Action OnPopupOpen;
        public Action<SDPopupComponentBase> OnPopupClose;
        public string MessageContent;
        public string MessageNoProfitContent;

        public static SDPopupData WelcomeBackMessage = new()
        {
            Priority = 10,
            PopupType = PopupTypes.WelcomeBackMessage,
            MessageContent = "Welcome Back!\r\nYour bakers worked tirelessly while \r\nyou were away. \r\nHere's a reward for their hard work!",
            MessageNoProfitContent = "Welcome Back! \r\nNo profits earned while away.\r\nHire bakers to make money \r\nwhile offline!",
        };

        public static SDPopupData StorePopupData = new()
        {
            Priority = 1,
            PopupType = PopupTypes.Store
        };

        public static SDPopupData FirstLoginMessage = new()
        {
            Priority = 10,
            PopupType = PopupTypes.FirstLoginMessage
        };

        public static SDPopupData LoadingAd = new()
        {
            Priority = 1,
            PopupType = PopupTypes.LoadingAd
        };

        public static SDPopupData LoadingAdFailed = new()
        {
            Priority = 11,
            PopupType = PopupTypes.LoadingAdFailed
        };
    }

    public enum PopupTypes
    {
        WelcomeBackMessage,
        Store,
        UpgradePopupMenu,
        FirstLoginMessage,
        LoadingAd,
        LoadingAdFailed
    }
}