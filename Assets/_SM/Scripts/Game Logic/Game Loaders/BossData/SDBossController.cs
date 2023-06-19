using UnityEngine;

namespace SD_GameLoad
{
    public class SDBossController
    {
        private SDBossDataManager BossDataManager => SDGameLogic.Instance.CurrentBossData;

        public void DamageBoss(double damage)
        {
            var currentBoss = BossDataManager.CurrentBoss?.BossInfo;
            if (currentBoss == null)
            {
                Debug.LogError("No current boss data available.");
                return;
            }

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

            currentBoss.Level++;
            currentBoss.TotalHp *= 1.1;
            currentBoss.CurrentHp = currentBoss.TotalHp;

            BossDataManager.SaveCurrentBossData();
        }
    }
}
