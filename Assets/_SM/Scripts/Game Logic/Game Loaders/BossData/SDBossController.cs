using UnityEngine;
using SD_Core;

namespace SD_GameLoad
{
    public class SDBossController
    {
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
            currentBoss.Index++;
            currentBoss.Level++;
            currentBoss.TotalHp *= 1.1;
            currentBoss.CurrentHp = currentBoss.TotalHp;
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.SpawnBoss, null);

            BossDataManager.SaveCurrentBossData();
        }
    }
}
