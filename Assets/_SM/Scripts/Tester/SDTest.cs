using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD_GameLoad;
using SD_Core;
using UnityEngine.UI;
using DG.Tweening;

namespace SD_Test
{
#if UNITY_EDITOR
    public class SDTest : SDLogicMonoBehaviour
    {
        // Update is called once per frame

        [SerializeField] Image healthBar;
        public double currentHealth = 1000;
        public double maxHealth = 1000;

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

            if(Input.GetKeyDown(KeyCode.C))
            {
                TakeDamage(100);
            }
        }

        public void TakeDamage(double damage)
        {
            currentHealth -= damage;
            UpdateHealthBar(currentHealth, maxHealth);
        }

        void UpdateHealthBar(double currentHealth, double maxHealth)
        {
            float fillAmount = (float)(currentHealth / maxHealth);
            healthBar.fillAmount = fillAmount;
        }
    }
#endif
}
