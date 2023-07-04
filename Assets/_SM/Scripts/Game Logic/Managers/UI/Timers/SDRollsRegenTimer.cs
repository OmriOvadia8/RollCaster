using UnityEngine;
using SD_Core;
using SD_GameLoad;
using System;
using DG.Tweening;
using TMPro;

namespace SD_UI
{
    public class SDRollsRegenTimer : SDLogicMonoBehaviour
    {
        [SerializeField] private TMP_Text rollRegenerationTimerText;
        [SerializeField] private GameObject timer;
        private int maxRolls;
        private int rollsRegenDuration;
        private const string ROLL_TWEEN = "roll";
        [SerializeField] int rollsRegenAmount = 10;

        private void OnEnable()
        {
            SDManager.Instance.EventsManager.AddListener(SDEventNames.OnPause, OnPause);
        }

        private void OnDisable()
        {
            SDManager.Instance.EventsManager.RemoveListener(SDEventNames.OnPause, OnPause);
        }

        private void Start()
        {
            timer.SetActive(true);
            maxRolls = GameLogic.PlayerController.GetMaxRollsAmount();
            rollsRegenDuration = GameLogic.Player.PlayerData.PlayerInfo.RollRegenDuration;
        }

        private void OnPause(object pauseStatus)
        {
            bool isPaused = (bool)pauseStatus;
            if (isPaused)
            {
                KillRollTweenTimer();
            }
            else
            {
                ActivateRollRegenerationAfterPause();
            }
        }

        private void KillRollTweenTimer() => DOTween.Kill(ROLL_TWEEN);

        private void StartRollRegenAfterPause(int durationAfterPause)
        {
             int remainingDuration = durationAfterPause;

             DOTween.To(() => remainingDuration, x => remainingDuration = x, 0, remainingDuration)
                 .SetEase(Ease.Linear).SetId(ROLL_TWEEN)
                 .OnUpdate(() =>
                 {
                     if (GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration != remainingDuration)
                     {
                         GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration = remainingDuration;
                         SDDebug.Log(GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration);
                         GameLogic.Player.SavePlayerData();
                     }
                     rollRegenerationTimerText.text = SDExtension.GetFormattedTimeSpan(remainingDuration);
                 })
                 .OnComplete(() => RollRegenCompletion());
        }

        public void ActivateRollRegenerationAfterPause()
        {
            int offlineTime = Manager.TimerManager.GetLastOfflineTimeSeconds();
            int currentRollsDuration = GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration;
            int newDuration;
            if(GameLogic.PlayerController.GetCurrentRollsAmount() <= maxRolls)
            {
                if (GameLogic.PlayerController.IsRegenOn())
                {
                    if (offlineTime < currentRollsDuration)
                    {
                        newDuration = currentRollsDuration - offlineTime;
                        StartRollRegenAfterPause(newDuration);
                    }

                    else
                    {
                        int timeAfterReward = offlineTime - currentRollsDuration;

                        int rewardsToAdd = (offlineTime + (rollsRegenDuration - currentRollsDuration)) / rollsRegenDuration;

                        for (int i = 0; i < rewardsToAdd; i++)
                        {
                            RollRegenReward();
                        }

                        int elapsedTimeAfterFullCycles = timeAfterReward % rollsRegenDuration;

                        newDuration = rollsRegenDuration - elapsedTimeAfterFullCycles;

                        StartRollRegenAfterPause(newDuration);
                    }
                }
                else
                {
                    StartRollRegeneration(rollsRegenDuration);
                }
            }
            
        }

        private void StartRollRegeneration(int currentDuration)
        {
            GameLogic.Player.PlayerData.PlayerInfo.IsRollRegenOn = true;
            rollRegenerationTimerText.text = SDExtension.GetFormattedTimeSpan(currentDuration);

            int remainingDuration = currentDuration;
            
            DOTween.To(() => remainingDuration, x => remainingDuration = x, 0, remainingDuration)
                .SetEase(Ease.Linear).SetId(ROLL_TWEEN)
                .OnUpdate(() =>
                {
                    if (GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration != remainingDuration)
                    {
                        GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration = remainingDuration;
                        SDDebug.Log(GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration);
                        GameLogic.Player.SavePlayerData();
                    }
                    rollRegenerationTimerText.text = SDExtension.GetFormattedTimeSpan(remainingDuration);
                })
                .OnComplete(() => RollRegenCompletion());
        }

        private void RollRegenCompletion()
        {
            rollRegenerationTimerText.text = SDExtension.GetFormattedTimeSpan(rollsRegenDuration);
            if (GameLogic.PlayerController.GetCurrentRollsAmount() < maxRolls)
            {
                RollRegenReward();
                StartRollRegeneration(rollsRegenDuration);
            }
            else
            {
                GameLogic.Player.PlayerData.PlayerInfo.IsRollRegenOn = false;
                GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration = rollsRegenDuration;
            }

            InvokeEvent(SDEventNames.UpdateRollsUI, null);

            GameLogic.Player.SavePlayerData();
        }

        private void RollRegenReward()
        {
            var currentRolls = GameLogic.PlayerController.GetCurrentRollsAmount();
            int potentialRollTotal = currentRolls + rollsRegenAmount;
            GameLogic.Player.PlayerData.PlayerInfo.CurrentRolls = Math.Min(potentialRollTotal, maxRolls);
            InvokeEvent(SDEventNames.UpdateRollsUI, null);
        }
    }
}