using System.Collections.Generic;
using System;
using UnityEngine;
using SD_Core;
using SD_GameLoad;

namespace SD_UI
{
    public class SDTextManager : SDLogicMonoBehaviour
    {
        [SerializeField] SDToastingManager toastingManager;

        private void OnEnable() => AddListener(SDEventNames.AbilityAnim, OnDamageToast);

        private void OnDisable() => RemoveListener(SDEventNames.AbilityAnim, OnDamageToast);

        private void OnDamageToast(object obj)
        {
            var eventData = (Tuple<AbilityNames, PoolNames>)obj;
            AbilityNames abilityName = eventData.Item1;
            PoolNames poolName = eventData.Item2;

            var ability = GameLogic.AbilityData.FindAbilityByName(abilityName.ToString());
            double damage = ability.Damage * (int)poolName;

            toastingManager.DisplayMoneyToast(damage, poolName);
        }

    }
}