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
            
            if(!BossDataManager.CurrentBoss.BossInfo.IsHPRegenOn)
            {
                SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.StartHPRegeneration, null);
            }

            if (currentBoss.CurrentHp <= 0)
            {
                ProgressToNextBossLevel();
            }

            BossDataManager.SaveCurrentBossData();
        }

        private void ProgressToNextBossLevel()
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
            currentBoss.XP = CalculateNewXp(currentBoss);

            if (!SDGameLogic.Instance.PlayerController.GetLevelUpFlag())
            {
                SDGameLogic.Instance.PlayerController.EarnAbilityPoints(PointsEarnTypes.BossKill);
            }

            SDGameLogic.Instance.PlayerController.SetLevelUpFlag(false);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateAllUpgradesButtons, null);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.SpawnBoss, null);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateBossLevelUI, null);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.StopHPRegen, null);
            BossDataManager.CurrentBoss.BossInfo.IsHPRegenOn = false;
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
