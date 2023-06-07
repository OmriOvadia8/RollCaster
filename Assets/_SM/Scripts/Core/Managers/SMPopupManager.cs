using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;

namespace SM_Core
{
    public class SMPopupManager
    {
        public List<SMPopupData> PopupsData = new();
        public Canvas popupsCanvas;

        private Dictionary<PopupTypes, SMPopupComponentBase> cachedPopups = new();

        public SMPopupManager() => CreateCanvas();

        private void CreateCanvas()
        {
            SMManager.Instance.FactoryManager.CreateAsync("PopupCanvas", Vector3.zero, (Canvas canvas) =>
            {
                popupsCanvas = canvas;
                Object.DontDestroyOnLoad(popupsCanvas);
            });
        }

        public void AddPopupToQueue(SMPopupData popupData)
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

        public void OpenPopup(SMPopupData dbPopupData)
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
                SMManager.Instance.FactoryManager.CreateAsync(dbPopupData.PopupType.ToString(),
                    Vector3.zero, (SMPopupComponentBase popupComponent) =>
                    {
                        cachedPopups.Add(dbPopupData.PopupType, popupComponent);
                        popupComponent.transform.SetParent(popupsCanvas.transform, false);
                        popupComponent.Init(dbPopupData);
                    });
            }
        }

        private void OnClosePopup(SMPopupComponentBase dbPopupComponentBase)
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
        public bool IsPopupActive(SMPopupData popupData)
        {
            if (cachedPopups.TryGetValue(popupData.PopupType, out SMPopupComponentBase popupComponent))
            {
                return popupComponent.gameObject.activeSelf;
            }

            return false;
        }
    }

    public class SMPopupData
    {
        public int Priority;
        public PopupTypes PopupType;
        public Action OnPopupOpen;
        public Action<SMPopupComponentBase> OnPopupClose;
        public string MessageContent;
        public string MessageNoProfitContent;

        public static SMPopupData WelcomeBackMessage = new()
        {
            Priority = 10,
            PopupType = PopupTypes.WelcomeBackMessage,
            MessageContent = "Welcome Back!\r\nYour bakers worked tirelessly while \r\nyou were away. \r\nHere's a reward for their hard work!",
            MessageNoProfitContent = "Welcome Back! \r\nNo profits earned while away.\r\nHire bakers to make money \r\nwhile offline!",
        };

        public static SMPopupData StorePopupData = new()
        {
            Priority = 1,
            PopupType = PopupTypes.Store
        };

        public static SMPopupData FirstLoginMessage = new()
        {
            Priority = 10,
            PopupType = PopupTypes.FirstLoginMessage
        };

        public static SMPopupData LoadingAd = new()
        {
            Priority = 1,
            PopupType = PopupTypes.LoadingAd
        };

        public static SMPopupData LoadingAdFailed = new()
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