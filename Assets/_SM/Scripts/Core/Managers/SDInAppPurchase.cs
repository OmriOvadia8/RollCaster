using System;
using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace SD_Core
{
    /// <summary>
    /// Handles all in-app purchases for the application.
    /// Implements IStoreListener interface to handle events related to the purchasing process.
    /// </summary>
    public class SDInAppPurchase : IStoreListener
    {
        private IStoreController storeController;
        private IExtensionProvider extensionProvider;
        private bool isInitialized;
        private Action<bool> purchaseCompleteAction;

        public SDInAppPurchase()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct("com.omri.rollcaster.rolls.small", ProductType.Consumable);
            builder.AddProduct("com.omri.rollcaster.rolls.big", ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);

            SDDebug.Log("SDInAppPurchase initialized.");
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            storeController = controller;
            extensionProvider = extensions;
            isInitialized = true;
            SDDebug.Log("OnInitialized called in SDInAppPurchase.");
        }

        /// <summary>
        /// Initiates a purchase of the specified product.
        /// </summary>
        /// <param name="productID">The ID of the product to purchase.</param>
        /// <param name="onPurchaseComplete">Action to be executed upon purchase completion.</param>
        public void Purchase(string productID, Action<bool> onPurchaseComplete)
        {
            if (!isInitialized)
            {
                SDDebug.Log("Attempted to purchase before initialization completed.");
                return;
            }
            purchaseCompleteAction = onPurchaseComplete;
            storeController.InitiatePurchase(productID);
        }

        public void OnInitializeFailed(InitializationFailureReason error) =>
            SDManager.Instance.CrashManager.LogExceptionHandling(error.ToString());

        public void OnInitializeFailed(InitializationFailureReason error, string message) =>
            SDManager.Instance.CrashManager.LogExceptionHandling(error.ToString() + "   " + message);

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var keys = new Dictionary<SDDataKeys, object>
            {
                {SDDataKeys.product_id, purchaseEvent.purchasedProduct.definition.id},
                {SDDataKeys.product_receipt, purchaseEvent.purchasedProduct.receipt}
            };

            SDManager.Instance.AnalyticsManager.ReportEvent(SDEventType.purchase_complete, keys);

            purchaseCompleteAction?.Invoke(true);
            purchaseCompleteAction = null;

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            if (!string.IsNullOrEmpty(product.receipt))
            {
                SDManager.Instance.CrashManager.LogBreadcrumb(product.receipt);
            }

            string failureMessage = failureReason.ToString();
            if (!string.IsNullOrEmpty(failureMessage))
            {
                SDManager.Instance.CrashManager.LogExceptionHandling(failureMessage);
            }

            purchaseCompleteAction?.Invoke(false);
            purchaseCompleteAction = null;
        }
    }
}