using SD_Core;
using SD_GameLoad;
using UnityEngine;

namespace SD_UI
{
    public class SDAnimationsOnToast : SDLogicMonoBehaviour
    {
        [SerializeField] SDToastingManager toastingManager;

        public void OnAnimation1(AbilityNames abilityName) => HandleAnimation(abilityName, PoolNames.DamageAnim1);

        public void OnAnimation2(AbilityNames abilityName)=> HandleAnimation(abilityName, PoolNames.DamageAnim2);

        public void OnAnimation3(AbilityNames abilityName) => HandleAnimation(abilityName, PoolNames.DamageAnim3);

        private void HandleAnimation(AbilityNames abilityName, PoolNames poolName)
        {
            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName.ToString());
            double damage = ability.Damage * (int)poolName;

            toastingManager.DisplayMoneyToast(damage, poolName);
            GameLogic.BossController.DamageBoss(damage);

            SDDebug.Log(CurrentBossInfo.CurrentHp);
            SDDebug.Log(CurrentBossInfo.Level);
        }
    }
}
