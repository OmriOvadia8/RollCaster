using SD_GameLoad;
using SD_Core;
using UnityEngine;

namespace SD_UI
{
    public class SDStoreActions : SDLogicMonoBehaviour
    {
        [SerializeField] GameObject failedToLoadWindow;

        public void IAPRollsPurchase(int productIndex)
        {
            var storeData = GameLogic.StoreManager.GetStoreByStoreID("1");
            GameLogic.StoreManager.TryBuyProduct(storeData.StoreProducts[productIndex].SKU, storeData.StoreID, isSuccess =>
            {
                if (isSuccess)
                {
                    GameLogic.Player.SavePlayerData();
                    InvokeEvent(SDEventNames.UpdateRollsUI, null);
                    InvokeEvent(SDEventNames.CheckRollsForSpin, null);
                    Manager.AnalyticsManager.ReportEvent(SDEventType.purchase_complete);
                    SDDebug.Log("IAP SUCCESS");
                }
                else
                {
                    SDDebug.LogException("IAP FAILED");
                    Manager.AnalyticsManager.ReportEvent(SDEventType.purchase_failed);
                }
            });
        }

        public void WatchAdForRolls(int rollsAmount)
        {
            if (Manager.AdsManager.IsRewardedAdReady())
            {
                Manager.AdsManager.ShowRewardedAd(success =>
                {
                    if (success)
                    {
                        GameLogic.PlayerController.IncreaseRoll(rollsAmount);
                    }
                    else
                    {
                        failedToLoadWindow.SetActive(true);
                    }
                });
            }
        }
    }
}