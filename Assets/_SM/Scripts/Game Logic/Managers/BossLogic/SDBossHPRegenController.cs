using DG.Tweening;
using SD_GameLoad;
using SD_Core;
using System;

namespace SD_Boss
{
    public class SDBossHPRegenController : SDLogicMonoBehaviour
    {
        private const string HP_TWEEN = "hp";
        private const float HP_PERCENT_REGEN = 0.05f;
        private int hpRegenDuration;

        private void OnEnable()
        {
            AddListener(SDEventNames.OnPause, HandlePause);
            AddListener(SDEventNames.StartHPRegeneration, ActivateHPRegenerationAfterPause);
            AddListener(SDEventNames.StopHPRegen, StopHPTweenTimer);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.OnPause, HandlePause);
            RemoveListener(SDEventNames.StartHPRegeneration, ActivateHPRegenerationAfterPause);
            RemoveListener(SDEventNames.StopHPRegen, StopHPTweenTimer);
        }

        private void Start()
        {
            hpRegenDuration = CurrentBossInfo.HPRegenDuration;
            ActivateHPRegenerationAfterPause();
        }

        private void HandlePause(object pauseStatus)
        {
            bool isPaused = (bool)pauseStatus;
            if (isPaused)
            {
                StopHPTweenTimer();
            }

            else
            {
                ActivateHPRegenerationAfterPause();
            }
        }

        private void StopHPTweenTimer(object obj = null) => DOTween.Kill(HP_TWEEN);

        private void StartHPRegenAfterPause(int durationAfterPause)
        {
            int remainingDuration = durationAfterPause;
            ConfigureHPTween(remainingDuration);
        }

        private void ConfigureHPTween(int remainingDuration)
        {
            DOTween.To(() => remainingDuration, x => remainingDuration = x, 0, remainingDuration)
                .SetEase(Ease.Linear).SetId(HP_TWEEN)
                .OnUpdate(() => UpdateHPRegenDuration(remainingDuration))
                .OnComplete(HPRegenCompletion);
        }

        private void UpdateHPRegenDuration(int remainingDuration)
        {
            if (CurrentBossInfo.CurrentHPRegenDuration != remainingDuration)
            {
                CurrentBossInfo.CurrentHPRegenDuration = remainingDuration;
                GameLogic.CurrentBossData.SaveCurrentBossData();
            }
        }

        public void ActivateHPRegenerationAfterPause(object obj = null)
        {
            int offlineTime = Manager.TimerManager.GetLastOfflineTimeSeconds();
            int currentHPDuration = CurrentBossInfo.CurrentHPRegenDuration;
            if (CurrentBossInfo.CurrentHp < CurrentBossInfo.TotalHp)
            {
                if (CurrentBossInfo.IsHPRegenOn)
                {
                    HandleRegen(offlineTime, currentHPDuration);
                }

                else
                {
                    StartHPRegeneration();
                }
            }
        }

        private void HandleRegen(int offlineTime, int currentHPDuration)
        {
            if (offlineTime < currentHPDuration)
            {
                StartHPRegenAfterPause(currentHPDuration - offlineTime);
            }
            else
            {
                CalculateHPAndStartRegen(offlineTime, currentHPDuration);
            }
        }

        private void CalculateHPAndStartRegen(int offlineTime, int currentHPDuration)
        {
            int timeAfterReward = offlineTime - currentHPDuration;
            int rewardsToAdd = (offlineTime + (hpRegenDuration - currentHPDuration)) / hpRegenDuration;

            for (int i = 0; i < rewardsToAdd; i++)
            {
                HPRegenReward();
            }

            int elapsedTimeAfterFullCycles = timeAfterReward % hpRegenDuration;
            int newDuration = hpRegenDuration - elapsedTimeAfterFullCycles;

            StartHPRegenAfterPause(newDuration);
        }

        private void StartHPRegeneration()
        {
            CurrentBossInfo.IsHPRegenOn = true;
            ConfigureHPTween(hpRegenDuration);
        }

        private void HPRegenCompletion()
        {
            if (CurrentBossInfo.CurrentHp < CurrentBossInfo.TotalHp)
            {
                HPRegenReward();
                StartHPRegeneration();
            }
            else
            {
                CurrentBossInfo.IsHPRegenOn = false;
                CurrentBossInfo.CurrentHPRegenDuration = hpRegenDuration;
            }

            UpdateHealthUI();
            GameLogic.CurrentBossData.SaveCurrentBossData();
        }

        private void HPRegenReward()
        {
            var currentHp = CurrentBossInfo.CurrentHp;
            double potentialHpTotal = currentHp + (CurrentBossInfo.TotalHp * HP_PERCENT_REGEN);
            CurrentBossInfo.CurrentHp = Math.Min(potentialHpTotal, CurrentBossInfo.TotalHp);
            UpdateHealthUI();
        }

        private void UpdateHealthUI() => InvokeEvent(SDEventNames.UpdateHealthUI, null);
    }
}