using UnityEngine;
using SD_Core;

namespace SD_GameLoad
{
    public class SDBossController
    {
        private const double BOSS_LEVEL_INCREASE_FACTOR = 2;
        private const double NORMAL_LEVEL_INCREASE_FACTOR = 1.1;
        private const double POST_BOSS_LEVEL_DECREASE_FACTOR = 0.7;
        private const int XP_INCREASE = 2;

        private SDBossDataManager BossDataManager => SDGameLogic.Instance.CurrentBossData;

        public void DamageBoss(double damage)
        {
            var currentBoss = BossDataManager.CurrentBoss?.BossInfo;
            if (currentBoss == null || !currentBoss.IsAlive)
            {
                Debug.LogError("No current boss data available or boss is already dead.");
                return;
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.HurtBoss, null);
            currentBoss.CurrentHp -= damage;

            if (currentBoss.CurrentHp <= 0)
            {
                ProgressToNextLevel();
            }

            BossDataManager.SaveCurrentBossData();
        }

        private void ProgressToNextLevel()
        {
            var currentBoss = BossDataManager.CurrentBoss?.BossInfo;
            if (currentBoss == null)
            {
                Debug.LogError("No current boss data available.");
                return;
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.KillBoss, null);
            SDGameLogic.Instance.PlayerController.AddPlayerXP(currentBoss.XP);
            currentBoss.Index++;
            currentBoss.Level++;
            currentBoss.TotalHp = CalculateNewHp(currentBoss);
            currentBoss.CurrentHp = currentBoss.TotalHp;
            currentBoss.XP *= XP_INCREASE;

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.SpawnBoss, null);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateLevelUI, null);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.BossCrownVisibility, null);
            BossDataManager.SaveCurrentBossData();
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

    }
}
