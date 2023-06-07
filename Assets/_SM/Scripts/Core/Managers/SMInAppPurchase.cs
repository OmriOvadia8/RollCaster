//using System;
//using System.Collections.Generic;
//using UnityEngine.Purchasing;

//namespace SM_Core
//{
//    public class SMInAppPurchase : IStoreListener
//    {
//        private IStoreController storeController;
//        private IExtensionProvider extensionProvider;
//        private bool isInitialized;
//        private Action<bool> purchaseCompleteAction;

//        public SMInAppPurchase()
//        {
//            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

//            builder.AddProduct("com.omri.deliciousbakery.stars.big", ProductType.Consumable);
//            builder.AddProduct("com.omri.deliciousbakery.stars.small", ProductType.Consumable);
//            builder.AddProduct("com.omri.deliciousbakery.stars.medium", ProductType.Consumable);

//            UnityPurchasing.Initialize(this, builder);

//            SMDebug.Log("SMInAppPurchase initialized.");
//        }

//        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//        {
//            storeController = controller;
//            extensionProvider = extensions;
//            isInitialized = true;
//            SMDebug.Log("OnInitialized called in SMInAppPurchase.");
//        }

//        public void Purchase(string productID, Action<bool> onPurchaseComplete)
//        {
//            if (!isInitialized)
//            {
//                SMDebug.Log("Attempted to purchase before initialization completed.");
//                return;
//            }
//            purchaseCompleteAction = onPurchaseComplete;
//            storeController.InitiatePurchase(productID);
//        }

//        public void OnInitializeFailed(InitializationFailureReason error) =>
//            SMManager.Instance.CrashManager.LogExceptionHandling(error.ToString());

//        public void OnInitializeFailed(InitializationFailureReason error, string message) =>
//            SMManager.Instance.CrashManager.LogExceptionHandling(error.ToString() + "   " + message);

//        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
//        {
//            var keys = new Dictionary<SMDataKeys, object>
//            {
//                {SMDataKeys.product_id, purchaseEvent.purchasedProduct.definition.id},
//                {SMDataKeys.product_receipt, purchaseEvent.purchasedProduct.receipt}
//            };

//            SMManager.Instance.AnalyticsManager.ReportEvent(SMEventType.purchase_complete, keys);

//            purchaseCompleteAction?.Invoke(true);
//            purchaseCompleteAction = null;

//            return PurchaseProcessingResult.Complete;
//        }

//        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//        {
//            if (!string.IsNullOrEmpty(product.receipt))
//            {
//                SMManager.Instance.CrashManager.LogBreadcrumb(product.receipt);
//            }

//            string failureMessage = failureReason.ToString();
//            if (!string.IsNullOrEmpty(failureMessage))
//            {
//                SMManager.Instance.CrashManager.LogExceptionHandling(failureMessage);
//            }

//            purchaseCompleteAction?.Invoke(false);
//            purchaseCompleteAction = null;
//        }
//    }
//}