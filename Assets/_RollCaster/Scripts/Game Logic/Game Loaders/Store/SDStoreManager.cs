using System;
using System.Collections.Generic;
using System.Linq;
using SD_Core;

namespace SD_GameLoad
{
    /// <summary>
    /// Manages the store configurations and buying products.
    /// </summary>
    public class SDStoreManager
    {
        public SDStoresConfigData StoresConfigData;

        public SDStoreManager() => LoadStoresConfig();

        private void LoadStoresConfig() =>
            SDManager.Instance.ConfigManager.GetConfigAsync("store_config", (SDStoresConfigData config) => StoresConfigData = config);

        /// <summary>
        /// Attempt to purchase a product from a store.
        /// </summary>
        /// <param name="sku">The SKU of the product.</param>
        /// <param name="storeID">The ID of the store.</param>
        /// <param name="onComplete">A callback function to be invoked when the operation is complete.</param>
        public void TryBuyProduct(string sku, string storeID, Action<bool> onComplete)
        {
            SDStoreData store = GetStoreByStoreID(storeID);
            if (store == null)
            {
                onComplete?.Invoke(false);
                throw new ArgumentException($"Store with ID '{storeID}' not found");
            }

            SDStoreProduct product = store.StoreProducts.FirstOrDefault(x => x.SKU == sku);
            if (product == null)
            {
                onComplete?.Invoke(false);
                throw new ArgumentException($"Product with SKU '{sku}' not found in store '{storeID}'");
            }

            SDManager.Instance.PurchaseManager.Purchase(sku, isSuccess =>
            {
                if (isSuccess)
                {
                    RedeemBundle(product.StoreBundle);
                }
                onComplete?.Invoke(isSuccess);
            });
        }

        /// <summary>
        /// Redeem a bundle in the product store.
        /// </summary>
        /// <param name="productStoreBundle">The bundle to redeem.</param>
        private void RedeemBundle(SDBundle[] productStoreBundle)
        {
            foreach (var bundle in productStoreBundle)
            {
                SDGameLogic.Instance.PlayerController.IncreaseRoll(bundle.RollIncreaseAmount);
            }
        }

        /// <summary>
        /// Retrieve a store data by its ID.
        /// </summary>
        /// <param name="storeID">The ID of the store.</param>
        /// <returns>The corresponding store data.</returns>
        public SDStoreData GetStoreByStoreID(string storeID)
        {
            return StoresConfigData.StoreDatas.FirstOrDefault(x => x.StoreID == storeID);
        }

        public class SDStoresConfigData
        {
            public List<SDStoreData> StoreDatas = new();
        }

        public class SDStoreData
        {
            public string StoreID;
            public string Title;
            public List<SDStoreProduct> StoreProducts = new();
        }

        public class SDStoreProduct
        {
            public string SKU;
            public string ArtName;
            public string SellingText;
            public SDBundle[] StoreBundle;
        }

        public class SDBundle
        {
            public int RollIncreaseAmount;
        }
    }
}