using SD_Core;
using System;
using SD_GameLoad;

namespace SD_UI
{
    public class SDAnimationsOnToast : SDMonoBehaviour
    {
        public void OnAnimation1() => Manager.EventsManager.InvokeEvent(SDEventNames.AbilityAnim, Tuple.Create(AbilityNames.SkullSmoke, PoolNames.DamageAnim1));

        public void OnAnimation2() => Manager.EventsManager.InvokeEvent(SDEventNames.AbilityAnim, Tuple.Create(AbilityNames.Slashes, PoolNames.DamageAnim2));

        public void OnAnimation3() => Manager.EventsManager.InvokeEvent(SDEventNames.AbilityAnim, Tuple.Create(AbilityNames.SmokeExplosion, PoolNames.DamageAnim3));
    }
}