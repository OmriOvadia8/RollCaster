using SD_Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD_UI
{
    public class SDAnimationsOnToast : SDMonoBehaviour
    {
        public void OnAnimation1()
        {
            double damage = 999;
            Color color = Color.blue;
            float size = 85;
            DamageEventDetails details = new DamageEventDetails(damage, color, size);
            Manager.EventsManager.InvokeEvent(SDEventNames.OnAbilityRolled, details);
        }

        public void OnAnimation2()
        {
            double damage = 999;
            Color color = Color.yellow;
            float size = 100;
            DamageEventDetails details = new DamageEventDetails(damage, color, size);
            Manager.EventsManager.InvokeEvent(SDEventNames.OnAbilityRolled, details);
        }

        public void OnAnimation3()
        {
            double damage = 999;
            Color color = Color.red;
            float size = 120;
            DamageEventDetails details = new DamageEventDetails(damage, color, size);
            Manager.EventsManager.InvokeEvent(SDEventNames.OnAbilityRolled, details);
        }
    }
}