using SD_Core;
using System;
using SD_GameLoad;

namespace SD_UI
{
    public class SDAnimationsOnToast : SDMonoBehaviour
    {
        public void OnAnimation1(AbilityNames abilityName)
        {
            Manager.EventsManager.InvokeEvent(SDEventNames.AbilityAnim, Tuple.Create(abilityName, PoolNames.DamageAnim1));
        }

        public void OnAnimation2(AbilityNames abilityName) => Manager.EventsManager.InvokeEvent(SDEventNames.AbilityAnim, Tuple.Create(abilityName, PoolNames.DamageAnim2));

        public void OnAnimation3(AbilityNames abilityName) => Manager.EventsManager.InvokeEvent(SDEventNames.AbilityAnim, Tuple.Create(abilityName, PoolNames.DamageAnim3));

        public void AnimationTest(AbilityNames abilityName, double damage)
        {
            Manager.EventsManager.InvokeEvent(SDEventNames.AbilityAnim, Tuple.Create(abilityName, damage, PoolNames.DamageAnim1));
        }
    }
}