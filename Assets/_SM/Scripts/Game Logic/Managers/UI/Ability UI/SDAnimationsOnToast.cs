using SD_Core;
using SD_GameLoad;
using UnityEngine;
using SD_Sound;

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

            toastingManager.DisplayTextToast(damage, poolName);
            GameLogic.BossController.DamageBoss(damage);

            InvokeEvent(SDEventNames.UpdateHealthUI, null);

            SDDebug.Log(CurrentBossInfo.CurrentHp);
            SDDebug.Log(CurrentBossInfo.Level);
        }

        public void PlayAbilitySound(SoundEffectType soundEffect) => InvokeEvent(SDEventNames.PlaySound, soundEffect);
    }
}
