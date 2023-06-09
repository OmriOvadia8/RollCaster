using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SD_Core
{
    public class SDTimeManager
    {
        private bool isLooping;

        private Dictionary<int, List<SDTimerData>> timerActions = new();
        private List<SDAlarmData> activeAlarms = new();

        private SDOfflineTime dbOfflineTime;

        private bool isPaused;
        private int counter;
        private int alarmCounter;
        private int offlineSeconds;

        public SDTimeManager()
        {
            _ = TimerLoop();

            SDManager.Instance.SaveManager.Load((SDOfflineTime data) =>
            {
                dbOfflineTime = data ?? new SDOfflineTime
                {
                    LastCheck = DateTime.UtcNow,
                };

                SDManager.Instance.SaveManager.Save(dbOfflineTime);
                CheckOfflineTime();
            });

            SDManager.Instance.EventsManager.AddListener(SDEventNames.OnPause, OnPause);
        }

        private void OnPause(object pauseStatus)
        {
            bool isNowPaused = (bool)pauseStatus;
            if (isNowPaused)
            {
                dbOfflineTime.LastCheck = DateTime.UtcNow;
                SDManager.Instance.SaveManager.Save(dbOfflineTime);
            }
            else
            {
                CheckOfflineTime();
            }
        }

        ~SDTimeManager()
        {
            isLooping = false;
            SDManager.Instance.EventsManager.RemoveListener(SDEventNames.OnPause, OnPause);
        }

        private void CheckOfflineTime()
        {
            var timePassed = DateTime.UtcNow - dbOfflineTime.LastCheck;
            offlineSeconds = Mathf.Clamp((int)timePassed.TotalSeconds, 0, 48.HoursToSeconds());

            dbOfflineTime.LastCheck = DateTime.UtcNow;
            SDManager.Instance.SaveManager.Save(dbOfflineTime);

            SDDebug.Log("OFFLINE TIME: " + offlineSeconds);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.OfflineTimeRefreshed, offlineSeconds);
        }

        public int GetLastOfflineTimeSeconds()
        {
            return offlineSeconds;
        }

        private async Task TimerLoop()
        {
            isLooping = true;

            while (isLooping)
            {
                await Task.Delay(1000);
                InvokeTime();
            }

            isLooping = false;
        }

        private void InvokeTime()
        {
            counter++;

            foreach (var timers in timerActions)
            {
                foreach (var timer in timers.Value)
                {
                    var offsetCounter = counter - timer.StartCounter;

                    if (offsetCounter % timers.Key == 0)
                    {
                        timer.TimerAction.Invoke();
                    }
                }
            }

            for (var index = 0; index < activeAlarms.Count; index++)
            {
                var alarmData = activeAlarms[index];

                if (DateTime.Compare(alarmData.AlarmTime, DateTime.UtcNow) < 0)
                {
                    alarmData.AlarmAction.Invoke();
                    activeAlarms.Remove(alarmData);
                }
            }
        }

        public void SubscribeTimer(int intervalSeconds, Action onTickAction)
        {
            if (!timerActions.ContainsKey(intervalSeconds))
            {
                timerActions.Add(intervalSeconds, new List<SDTimerData>());
            }

            timerActions[intervalSeconds].Add(new SDTimerData(counter, onTickAction));
        }

        public void UnSubscribeTimer(int intervalSeconds, Action onTickAction)
        {
            timerActions[intervalSeconds].RemoveAll(x => x.TimerAction == onTickAction);
        }

        public int SetAlarm(int seconds, Action onAlarmAction)
        {
            alarmCounter++;

            var alarmData = new SDAlarmData
            {
                ID = alarmCounter,
                AlarmTime = DateTime.UtcNow.AddSeconds(seconds),
                AlarmAction = onAlarmAction
            };

            activeAlarms.Add(alarmData);
            return alarmCounter;
        }

        public void DisableAlarm(int alarmID)
        {
            activeAlarms.RemoveAll(x => x.ID == alarmID);
        }

        public int GetLeftOverTime(OfflineTimes timeType)
        {
            if (!dbOfflineTime.LeftOverTimes.ContainsKey(timeType))
            {
                return 0;
            }

            return dbOfflineTime.LeftOverTimes[timeType];
        }

        public void SetLeftOverTime(OfflineTimes timeType, int timeAmount) => dbOfflineTime.LeftOverTimes[timeType] = timeAmount;
    }

    public class SDTimerData
    {
        public Action TimerAction;
        public int StartCounter;

        public SDTimerData(int counter, Action onTickAction)
        {
            TimerAction = onTickAction;
            StartCounter = counter;
        }
    }

    public class SDAlarmData
    {
        public int ID;
        public DateTime AlarmTime;
        public Action AlarmAction;
    }

    [Serializable]
    public class SDOfflineTime : ISDSaveData
    {
        public DateTime LastCheck;
        public Dictionary<OfflineTimes, int> LeftOverTimes = new();
    }

    public enum OfflineTimes
    {
        DailyBonus,
        ExtraBonus
    }
}