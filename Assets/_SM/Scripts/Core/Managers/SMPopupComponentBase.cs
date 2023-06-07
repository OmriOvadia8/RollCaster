using System.Collections.Generic;

namespace SM_Core
{
    public class SMPopupComponentBase : SMMonoBehaviour
    {
        protected SMPopupData popupData;

        public virtual void Init(SMPopupData popupData)
        {
            this.popupData = popupData;
            OnOpenPopup();
        }

        protected virtual void OnOpenPopup()
        {
            var data = new Dictionary<SMDataKeys, object>();
            data.Add(SMDataKeys.popup_type, popupData.PopupType.ToString());
            Manager.AnalyticsManager.ReportEvent(SMEventType.popup_open, data);

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
                SMDebug.LogException("closepopup execption");
            }
        }

        protected virtual void OnClosePopup()
        {
            if (popupData == null)
            {
                SMDebug.LogException("Onclosepopup execption");
                return;
            }

            var data = new Dictionary<SMDataKeys, object>();
            data.Add(SMDataKeys.popup_type, popupData.PopupType.ToString());
            Manager.AnalyticsManager.ReportEvent(SMEventType.popup_close, data);

            popupData.OnPopupClose?.Invoke(this);
        }
    }
}