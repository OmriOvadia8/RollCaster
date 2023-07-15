using SD_GameLoad;
using SD_Core;

namespace SD_UI
{
    /// <summary>
    /// The SDStoreActions class handles the in-app purchases and ad watching reward within the game.
    /// </summary>
    public class SDStoreActions : SDLogicMonoBehaviour
    {
        /// <summary>
        /// Handles the in-app purchase of rolls based on the product index.
        /// </summary>
        /// <param name="productIndex">The index of the product in the store's product list.</param>
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

        /// <summary>
        /// Allows the player to watch an ad in exchange for a specified amount of rolls.
        /// </summary>
        /// <param name="rollsAmount">The amount of rolls the player receives after watching the ad.</param>
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
                        InvokeEvent(SDEventNames.FailAdToast, null);
                    }
                });
            }

            else
            {
                InvokeEvent(SDEventNames.FailAdToast, null);
            }
        }
    }
}