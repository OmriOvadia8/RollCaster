using SD_GameLoad;
using SD_Core;

namespace SD_UI
{
    public class SDStoreActions : SDLogicMonoBehaviour
    {
        public void IAPStarsPurchase(int productIndex)
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
    }
}