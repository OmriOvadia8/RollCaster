using UnityEngine;
using SD_Core;

namespace SD_GameLoad
{
    public class SDBossController
    {
        private const double BOSS_LEVEL_INCREASE_FACTOR = 2;
        private const double NORMAL_LEVEL_INCREASE_FACTOR = 1.1;
        private const double POST_BOSS_LEVEL_DECREASE_FACTOR = 0.7;
        private const double BOSS_XP_INCREASE_FACTOR = 1.03;
        private const double SPECIAL_BOSS_XP_INCREASE_FACTOR = 1.22;

        private SDBossDataManager BossDataManager => SDGameLogic.Instance.CurrentBossData;

        /// <summary>
        /// Apply damage to the current boss and progress to next boss level if current boss's health is below or equal to zero.
        /// </summary>
        public void DamageBoss(double damage)
        {
            var currentBoss = BossDataManager.CurrentBoss?.BossInfo;

            if (currentBoss == null || !currentBoss.IsAlive)
            {
                Debug.LogError("No current boss data available or boss is already dead.");
                return;
            }

            InvokeEvent(SDEventNames.HurtBoss);
            currentBoss.CurrentHp -= damage;

            if (!currentBoss.IsHPRegenOn)
            {
                InvokeEvent(SDEventNames.StartHPRegeneration);
            }

            if (currentBoss.CurrentHp <= 0)
            {
                ProgressToNextBossLevel();
            }

            BossDataManager.SaveCurrentBossData();
        }

        private void InvokeEvent(SDEventNames eventName)
        {
            SDManager.Instance.EventsManager.InvokeEvent(eventName, null);
        }

        private void ProgressToNextBossLevel()
        {
            var currentBoss = BossDataManager.CurrentBoss?.BossInfo;

            if (currentBoss == null)
            {
                Debug.LogError("No current boss data available.");
                return;
            }

            HandleBossKill(currentBoss);
            HandleNewBossSpawn();

            currentBoss.IsHPRegenOn = false;
            SDManager.Instance.AnalyticsManager.ReportEvent(SDEventType.boss_killed);
            BossDataManager.SaveCurrentBossData();
        }

        /// <summary>
        /// Invoke events and perform operations related to boss kill.
        /// </summary>
        private void HandleBossKill(SDBossData currentBoss)
        {
            InvokeEvent(SDEventNames.KillBoss);
            SDGameLogic.Instance.PlayerController.AddPlayerXP(currentBoss.XP);
            currentBoss.Index++;
            currentBoss.Level++;

            currentBoss.TotalHp = CalculateNewHp(currentBoss);
            currentBoss.CurrentHp = currentBoss.TotalHp;
            currentBoss.XP = CalculateNewXp(currentBoss);

            if (!SDGameLogic.Instance.PlayerController.GetLevelUpFlag())
            {
                SDGameLogic.Instance.PlayerController.EarnAbilityPoints(PointsEarnTypes.BossKill);
            }

            SDGameLogic.Instance.PlayerController.SetLevelUpFlag(false);
        }

        /// <summary>
        /// Invoke events and perform operations related to spawning a new boss
        /// </summary>
        private void HandleNewBossSpawn()
        {
            InvokeEvent(SDEventNames.UpdateAllUpgradesButtons);
            InvokeEvent(SDEventNames.SpawnBoss);
            InvokeEvent(SDEventNames.UpdateBossLevelUI);
            InvokeEvent(SDEventNames.StopHPRegen);
        }

        private double CalculateNewHp(SDBossData currentBoss)
        {
            if (currentBoss.Level % currentBoss.SpecialBossLevel == 0)
            {
                return currentBoss.TotalHp * BOSS_LEVEL_INCREASE_FACTOR;
            }
            else if (currentBoss.Level % currentBoss.SpecialBossLevel == 1)
            {
                return currentBoss.TotalHp * POST_BOSS_LEVEL_DECREASE_FACTOR;
            }
            else
            {
                return currentBoss.TotalHp * NORMAL_LEVEL_INCREASE_FACTOR;
            }
        }

        private double CalculateNewXp(SDBossData currentBoss)
        {
            if (currentBoss.Level % currentBoss.SpecialBossLevel == 0)
            {
                return currentBoss.XP * SPECIAL_BOSS_XP_INCREASE_FACTOR;
            }
            else
            {
                return currentBoss.XP * BOSS_XP_INCREASE_FACTOR;
            }
        }
    }
}
