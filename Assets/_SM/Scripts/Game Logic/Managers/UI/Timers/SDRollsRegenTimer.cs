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
        private const string ROLL_TWEEN = "roll";
        private const int ROLLS_REGEN_AMOUNT = 5;
        private int maxRolls;
        private int rollsRegenDuration;

        private void OnEnable()
        {
            AddListener(SDEventNames.OnPause, HandlePause);
            AddListener(SDEventNames.StartRollsRegeneration, ActivateRollRegenerationAfterPause);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.OnPause, HandlePause);
            RemoveListener(SDEventNames.StartRollsRegeneration, ActivateRollRegenerationAfterPause);
        }

        private void Start()
        {
            maxRolls = GameLogic.PlayerController.GetMaxRollsAmount();
            rollsRegenDuration = GameLogic.Player.PlayerData.PlayerInfo.RollRegenDuration;
            CheckTimerCancel();
            timer.SetActive(false);
            ActivateRollRegenerationAfterPause();
        }

        private void HandlePause(object pauseStatus)
        {
            bool isPaused = (bool)pauseStatus;
            if (isPaused)
            {
                StopRollTweenTimer();
            }
            else
            {
                ActivateRollRegenerationAfterPause();
            }
        }

        private void StopRollTweenTimer() => DOTween.Kill(ROLL_TWEEN);

        private void StartRollRegenAfterPause(int durationAfterPause)
        {
            int remainingDuration = durationAfterPause;
            ConfigureRollTween(remainingDuration);
        }

        private void ConfigureRollTween(int remainingDuration)
        {
            DOTween.To(() => remainingDuration, x => remainingDuration = x, 0, remainingDuration)
                .SetEase(Ease.Linear).SetId(ROLL_TWEEN)
                .OnUpdate(() => UpdateRollRegenDuration(remainingDuration))
                .OnComplete(RollRegenCompletion);
        }

        private void UpdateRollRegenDuration(int remainingDuration)
        {
            if (GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration != remainingDuration)
            {
                GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration = remainingDuration;
                GameLogic.Player.SavePlayerData();
            }
            rollRegenerationTimerText.text = $"+{ROLLS_REGEN_AMOUNT} Rolls in {SDExtension.GetFormattedTimeSpan(remainingDuration)}";
        }

        public void ActivateRollRegenerationAfterPause(object obj = null)
        {
            int offlineTime = Manager.TimerManager.GetLastOfflineTimeSeconds();
            int currentRollsDuration = GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration;
            if (GameLogic.PlayerController.GetCurrentRollsAmount() < maxRolls)
            {
                timer.SetActive(true);
                if (GameLogic.PlayerController.IsRegenOn())
                {
                    HandleRegen(offlineTime, currentRollsDuration);
                }
                else
                {
                    StartRollRegeneration();
                }
            }
        }

        private void HandleRegen(int offlineTime, int currentRollsDuration)
        {
            if (offlineTime < currentRollsDuration)
            {
                StartRollRegenAfterPause(currentRollsDuration - offlineTime);
            }
            else
            {
                CalculateRewardsAndStartRegen(offlineTime, currentRollsDuration);
            }
        }

        private void CalculateRewardsAndStartRegen(int offlineTime, int currentRollsDuration)
        {
            int timeAfterReward = offlineTime - currentRollsDuration;
            int rewardsToAdd = (offlineTime + (rollsRegenDuration - currentRollsDuration)) / rollsRegenDuration;

            for (int i = 0; i < rewardsToAdd; i++) RollRegenReward();

            int elapsedTimeAfterFullCycles = timeAfterReward % rollsRegenDuration;
            int newDuration = rollsRegenDuration - elapsedTimeAfterFullCycles;

            StartRollRegenAfterPause(newDuration);
        }

        private void StartRollRegeneration()
        {
            GameLogic.Player.PlayerData.PlayerInfo.IsRollRegenOn = true;
            rollRegenerationTimerText.text = SDExtension.GetFormattedTimeSpan(rollsRegenDuration);
            ConfigureRollTween(rollsRegenDuration);
        }

        private void RollRegenCompletion()
        {
            rollRegenerationTimerText.text = SDExtension.GetFormattedTimeSpan(rollsRegenDuration);
            if (GameLogic.PlayerController.GetCurrentRollsAmount() < maxRolls)
            {
                RollRegenReward();
                StartRollRegeneration();
            }
            else
            {
                GameLogic.Player.PlayerData.PlayerInfo.IsRollRegenOn = false;
                GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration = rollsRegenDuration;
                timer.SetActive(false);
            }
            UpdateRollsUI();
            GameLogic.Player.SavePlayerData();
        }

        private void RollRegenReward()
        {
            var currentRolls = GameLogic.PlayerController.GetCurrentRollsAmount();
            int potentialRollTotal = currentRolls + ROLLS_REGEN_AMOUNT;
            GameLogic.Player.PlayerData.PlayerInfo.CurrentRolls = Math.Min(potentialRollTotal, maxRolls);
            UpdateRollsUI();
        }

        private void UpdateRollsUI()
        {
            InvokeEvent(SDEventNames.UpdateRollsUI, null);
            InvokeEvent(SDEventNames.CheckRollsForSpin, null);
        }

        private void CheckTimerCancel()
        {
            if(GameLogic.PlayerController.GetCurrentRollsAmount() >= maxRolls)
            {
                GameLogic.Player.PlayerData.PlayerInfo.IsRollRegenOn = false;
                GameLogic.Player.SavePlayerData();
            }
        }
    }
}
