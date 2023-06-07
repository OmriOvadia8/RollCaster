using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SM_Core
{
    public class SMTimeManager
    {
        private bool isLooping;

        private Dictionary<int, List<SMTimerData>> timerActions = new();
        private List<SMAlarmData> activeAlarms = new();

        private SMOfflineTime dbOfflineTime;

        private bool isPaused;
        private int counter;
        private int alarmCounter;
        private int offlineSeconds;

        public SMTimeManager()
        {
            _ = TimerLoop();

            SMManager.Instance.SaveManager.Load((SMOfflineTime data) =>
            {
                dbOfflineTime = data ?? new SMOfflineTime
                {
                    LastCheck = DateTime.UtcNow,
                };

                SMManager.Instance.SaveManager.Save(dbOfflineTime);
                CheckOfflineTime();
            });

            SMManager.Instance.EventsManager.AddListener(SMEventNames.OnPause, OnPause);
        }

        private void OnPause(object pauseStatus)
        {
            bool isNowPaused = (bool)pauseStatus;
            if (isNowPaused)
            {
                dbOfflineTime.LastCheck = DateTime.UtcNow;
                SMManager.Instance.SaveManager.Save(dbOfflineTime);
            }
            else
            {
                CheckOfflineTime();
            }
        }

        ~SMTimeManager()
        {
            isLooping = false;
            SMManager.Instance.EventsManager.RemoveListener(SMEventNames.OnPause, OnPause);
        }

        private void CheckOfflineTime()
        {
            var timePassed = DateTime.UtcNow - dbOfflineTime.LastCheck;
            offlineSeconds = Mathf.Clamp((int)timePassed.TotalSeconds, 0, 48.HoursToSeconds());

            dbOfflineTime.LastCheck = DateTime.UtcNow;
            SMManager.Instance.SaveManager.Save(dbOfflineTime);

            SMDebug.Log("OFFLINE TIME: " + offlineSeconds);
            SMManager.Instance.EventsManager.InvokeEvent(SMEventNames.OfflineTimeRefreshed, offlineSeconds);
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
                timerActions.Add(intervalSeconds, new List<SMTimerData>());
            }

            timerActions[intervalSeconds].Add(new SMTimerData(counter, onTickAction));
        }

        public void UnSubscribeTimer(int intervalSeconds, Action onTickAction)
        {
            timerActions[intervalSeconds].RemoveAll(x => x.TimerAction == onTickAction);
        }

        public int SetAlarm(int seconds, Action onAlarmAction)
        {
            alarmCounter++;

            var alarmData = new SMAlarmData
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

    public class SMTimerData
    {
        public Action TimerAction;
        public int StartCounter;

        public SMTimerData(int counter, Action onTickAction)
        {
            TimerAction = onTickAction;
            StartCounter = counter;
        }
    }

    public class SMAlarmData
    {
        public int ID;
        public DateTime AlarmTime;
        public Action AlarmAction;
    }

    [Serializable]
    public class SMOfflineTime : ISMSaveData
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