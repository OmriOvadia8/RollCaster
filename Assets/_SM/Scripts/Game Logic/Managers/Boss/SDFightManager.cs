using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD_Core;
using SD_GameLoad;

namespace SD_Fight
{
    public class SDFightManager : SDLogicMonoBehaviour
    { 

        private void OnEnable()
        {
            AddListener(SDEventNames.CastDamage, DoDamage);
            AddListener(SDEventNames.CastDamage, DoDamageToast);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.CastDamage, DoDamage);
            RemoveListener(SDEventNames.CastDamage, DoDamageToast);
        }

        private void DoDamageToast(object damageAmount)
        {
            double abilityDamage = (double)damageAmount;
        }

        private void DoDamage(object damageAmount)
        {
            double abilityDamage = (double)damageAmount;
            GameLogic.BossController.DamageBoss(abilityDamage);
        }
    }
}