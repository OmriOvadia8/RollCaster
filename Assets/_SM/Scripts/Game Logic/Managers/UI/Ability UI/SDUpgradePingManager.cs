using UnityEngine;
using SD_GameLoad;
using SD_Core;

namespace SD_UI
{
    public class SDUpgradePingManager : SDLogicMonoBehaviour
    {
        [SerializeField] GameObject[] upgradePing;
        [SerializeField] GameObject abilityTabPing;

        public void UpdateAbilityTabPing()
        {
            for (int i = 0; i < upgradePing.Length; i++)
            {
                if (upgradePing[i].activeSelf)
                {
                    abilityTabPing.SetActive(true);
                    return;
                }
            }

            abilityTabPing.SetActive(false);
        }

        public void SetPing(int index, bool state)
        {
            if (index < upgradePing.Length)
            {
                upgradePing[index].SetActive(state);
            }
            else
            {
                SDDebug.LogException("The provided index is out of bounds of upgradePing array.");
            }
        }
    }
}
