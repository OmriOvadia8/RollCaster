using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine.Device;

namespace SD_Core
{
    /// <summary>
    /// Class used to report analytics data to Firebase Analytics.
    /// </summary>
    public class SDAnalyticsManager
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SDAnalyticsManager"/> class.
        /// </summary>
        public SDAnalyticsManager() => SetUserID();

        /// <summary>
        /// Reports an event with a specific type to Firebase Analytics.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        public void ReportEvent(SDEventType eventType)
        {
            ReportEvent(eventType, new Dictionary<SDDataKeys, object>());
        }

        /// <summary>
        /// Reports an event with a specific type and additional data to Firebase Analytics.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="data">The additional data associated with the event.</param>
        public void ReportEvent(SDEventType eventType, Dictionary<SDDataKeys, object> data)
        {
            var paramsData = new List<Parameter>();

            foreach (var keyVal in data)
            {
                if (keyVal.Value == null)
                {
                    continue;
                }

                var objType = keyVal.Value.GetType();

                var keyName = keyVal.Key.ToString();
                if (objType == typeof(string))
                {
                    paramsData.Add(new Parameter(keyName, (string)keyVal.Value));
                }
                else if (objType == typeof(float))
                {
                    paramsData.Add(new Parameter(keyName, (float)keyVal.Value));
                }
                else if (objType == typeof(int))
                {
                    paramsData.Add(new Parameter(keyName, (int)keyVal.Value));
                }
                else if (objType == typeof(bool))
                {
                    paramsData.Add(new Parameter(keyName, (bool)keyVal.Value ? 1 : 0));
                }
            }

            FirebaseAnalytics.LogEvent(eventType.ToString(), paramsData.ToArray());
        }

        /// <summary>
        /// Sets the user properties to Firebase Analytics.
        /// </summary>
        /// <param name="data">A dictionary containing the property names and values.</param>
        public void SetUserProperties(Dictionary<string, string> data)
        {
            foreach (var keyVal in data)
            {
                SetUserProperty(keyVal.Key, keyVal.Value);
            }
        }

        /// <summary>
        /// Sets a user property to Firebase Analytics.
        /// </summary>
        /// <param name="key">The name of the property.</param>
        /// <param name="val">The value of the property.</param>
        public void SetUserProperty(string key, string val)
        {
            FirebaseAnalytics.SetUserProperty(key, val);
        }

        /// <summary>
        /// Sets the user ID to the unique identifier of the device.
        /// </summary>
        private void SetUserID()
        {
            FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
        }
    }

    /// <summary>
    /// Enumerates the possible types of events that can be reported.
    /// </summary>
    public enum SDEventType
    {
        app_loaded,
        upgrade_item,
        try_upgrade_out_of,
        ad_show_start,
        ad_show_click,
        ad_show_complete,
        purchase_complete,
        product_unknown,
        purchase_failed,
        unity_services_failed,
        boss_killed
    }

    /// <summary>
    /// Enumerates the possible keys of the data that can be reported.
    /// </summary>
    public enum SDDataKeys
    {
        type_id,
        upgrade_level,
        popup_type,
        product_id,
        product_receipt
    }
}
