using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD_GameLoad;

namespace SD_Boss
{
    public class SDBossSpawner : SDLogicMonoBehaviour
    {
        private const int YELLOW_BOSS = 0;
        private const int BLUE_BOSS = 1;
        private const int RED_BOSS = 2;

        private void Start()
        {
            SpawnBoss(CurrentBossInfo.Level);
        }

        public void SpawnBoss(int level)
        {
            int bossIndex = level % 10 == 0 ? RED_BOSS : level % 2;
        }
    }
}