using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD_GameLoad;
using SD_Core;

namespace SD_Test
{
#if UNITY_EDITOR
    public class SDTest : SDLogicMonoBehaviour
    {
        // Update is called once per frame

        private void Start()
        {
            SDDebug.Log(GameLogic.CurrentBossData.CurrentBoss.BossInfo.Level);
            SDDebug.Log(GameLogic.CurrentBossData.CurrentBoss.BossInfo.CurrentHp);
            SDDebug.Log(GameLogic.CurrentBossData.CurrentBoss.BossInfo.TotalHp);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                GameLogic.BossController.DamageBoss(100);
                SDDebug.Log(GameLogic.CurrentBossData.CurrentBoss.BossInfo.CurrentHp);
                SDDebug.Log(GameLogic.CurrentBossData.CurrentBoss.BossInfo.Level);
            }
        }
    }
#endif
}
