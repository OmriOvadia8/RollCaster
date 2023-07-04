using UnityEngine;
using SD_GameLoad;
using SD_Core;
using UnityEngine.UI;
using SD_Boss;
using SD_UI;

namespace SD_Test
{
#if UNITY_EDITOR
    public class SDTest : SDLogicMonoBehaviour
    {
        [SerializeField] Image healthBar;
        [SerializeField] SDBossAnimationsController bossAnim;
        [SerializeField] SDToastingManager toasting;
        public double currentHealth = 1000;
        public double maxHealth = 1000;

        private void Start()
        {
            SDDebug.Log(CurrentBossInfo.Level);
            SDDebug.Log(CurrentBossInfo.CurrentHp);
            SDDebug.Log(CurrentBossInfo.TotalHp);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                GameLogic.BossController.DamageBoss(100);
                SDDebug.Log(CurrentBossInfo.CurrentHp);
                SDDebug.Log(CurrentBossInfo.Level);
                SDDebug.Log(CurrentBossInfo.IsAlive);
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                bossAnim.HurtBoss();
                TakeDamage(CurrentBossInfo.CurrentHp - (CurrentBossInfo.CurrentHp -1));
                SDDebug.Log("index" + CurrentBossInfo.Index);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                SDDebug.Log(GameLogic.Player.PlayerData.PlayerInfo.Level);
                SDDebug.Log(GameLogic.Player.PlayerData.PlayerInfo.TotalXpRequired);
                SDDebug.Log(GameLogic.Player.PlayerData.PlayerInfo.CurrentXp);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                GameLogic.PlayerController.AddPlayerXP(500);
                GameLogic.Player.SavePlayerData();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                GameLogic.PlayerController.EarnAbilityPoints(PointsEarnTypes.BossKill);
                SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.CheckUnlockAbility, GameLogic.Player.PlayerData.PlayerInfo.Level);
                GameLogic.Player.SavePlayerData();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                toasting.DisplayTextToast(10, PoolNames.XPToast);
                toasting.DisplayTextToast(10, PoolNames.EarnPointsToast);
                toasting.DisplayTextToast(10, PoolNames.SpendPointsToast);
                toasting.DisplayTextToast(10, PoolNames.LevelUpToast);
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                SDDebug.Log(GameLogic.PlayerController.GetCurrentRollsAmount());
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
