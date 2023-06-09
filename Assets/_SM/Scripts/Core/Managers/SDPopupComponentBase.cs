using System.Collections.Generic;

namespace SD_Core
{
    public class SDPopupComponentBase : SDMonoBehaviour
    {
        protected SDPopupData popupData;

        public virtual void Init(SDPopupData popupData)
        {
            this.popupData = popupData;
            OnOpenPopup();
        }

        protected virtual void OnOpenPopup()
        {
            var data = new Dictionary<SDDataKeys, object>();
            data.Add(SDDataKeys.popup_type, popupData.PopupType.ToString());
            Manager.AnalyticsManager.ReportEvent(SDEventType.popup_open, data);

            popupData.OnPopupOpen?.Invoke();
        }

        public virtual void ClosePopup()
        {
            if (popupData != null)
            {
                OnClosePopup();
            }

            else
            {
                SDDebug.LogException("closepopup execption");
            }
        }

        protected virtual void OnClosePopup()
        {
            if (popupData == null)
            {
                SDDebug.LogException("Onclosepopup execption");
                return;
            }

            var data = new Dictionary<SDDataKeys, object>();
            data.Add(SDDataKeys.popup_type, popupData.PopupType.ToString());
            Manager.AnalyticsManager.ReportEvent(SDEventType.popup_close, data);

            popupData.OnPopupClose?.Invoke(this);
        }
    }
}