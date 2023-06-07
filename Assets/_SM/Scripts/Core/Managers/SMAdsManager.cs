//using System;
//using UnityEngine.Advertisements;

//namespace SM_Core
//{
//    public class SMAdsManager : IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
//    {
//        private bool isInterstitialLoaded;
//        private bool isRewardedLoaded;
//        private Action<bool> onShowCompleteAction;
//        private Action<bool> onShowRewardedCompleteAction;
//        private string gameID;
//        private string adUnit;
//        private string rewardedAdUnit;

//        public SMAdsManager() => LoadAdsConfig();

//        private void LoadAdsConfig()
//        {
//            SMManager.Instance.ConfigManager.GetConfigAsync("ads_config", (AdsConfigData config) =>
//            {
//                gameID = config.GameId;
//                adUnit = config.AdUnit;
//                rewardedAdUnit = config.RewardedAdUnit;

//                Advertisement.Initialize(gameID, false, this);
//                LoadAd();
//            });
//        }

//        public void ShowAd(Action<bool> onShowAdComplete = null)
//        {
//            if (!isInterstitialLoaded)
//            {
//                onShowAdComplete?.Invoke(false);
//                return;
//            }

//            onShowCompleteAction = onShowAdComplete;
//            Advertisement.Show(adUnit, this);
//        }

//        public void ShowRewardedAd(Action<bool> onShowAdComplete)
//        {
//            if (!isRewardedLoaded)
//            {
//                onShowAdComplete.Invoke(false);
//                return;
//            }

//            onShowRewardedCompleteAction = onShowAdComplete;
//            Advertisement.Show(rewardedAdUnit, this);
//        }

//        private void LoadAd()
//        {
//            Advertisement.Load(rewardedAdUnit, this);
//            Advertisement.Load(adUnit, this);
//        }

//        public void OnInitializationComplete() => LoadAd();

//        public void OnInitializationFailed(UnityAdsInitializationError error, string message) =>
//            SMManager.Instance.CrashManager.LogExceptionHandling("ads initialization failed " + message + " error : " + error.ToString());
        
//        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
//        {
//            SMManager.Instance.CrashManager.LogExceptionHandling(message);

//            onShowCompleteAction?.Invoke(false);
//            onShowRewardedCompleteAction?.Invoke(false);

//            onShowCompleteAction = null;
//            onShowRewardedCompleteAction = null;

//            LoadAd();
//        }

//        public void OnUnityAdsShowStart(string placementId) => SMManager.Instance.AnalyticsManager.ReportEvent(SMEventType.ad_show_start);

//        public void OnUnityAdsShowClick(string placementId)
//        {
//            SMManager.Instance.AnalyticsManager.ReportEvent(SMEventType.ad_show_click);
//            onShowCompleteAction?.Invoke(true);
//        }

//        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
//        {
//            SMManager.Instance.AnalyticsManager.ReportEvent(SMEventType.ad_show_complete);
//            onShowCompleteAction?.Invoke(true);

//            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED && placementId == rewardedAdUnit)
//            {
//                onShowRewardedCompleteAction?.Invoke(true);
//            }

//            onShowCompleteAction = null;
//            onShowRewardedCompleteAction = null;

//            LoadAd();
//        }

//        public void OnUnityAdsAdLoaded(string placementId)
//        {
//            if (placementId == rewardedAdUnit)
//            {
//                isRewardedLoaded = true;
//            }

//            else if (placementId == adUnit)
//            {
//                isInterstitialLoaded = true;
//            }
//        }

//        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
//        {
//            SMManager.Instance.CrashManager.LogExceptionHandling("load ad failed " + message + " error : " + error.ToString());

//            if (placementId == rewardedAdUnit)
//            {
//                isRewardedLoaded = false;
//            }
//            else if (placementId == adUnit)
//            {
//                isInterstitialLoaded = false;
//            }
            
//            LoadAd();
//        }

//        public bool IsRewardedAdReady()
//        {
//            return isRewardedLoaded;
//        }

//        public bool IsAdReady()
//        {
//            return isInterstitialLoaded;
//        }
//    }

//    public class AdsConfigData
//    {
//        public string GameId { get; set; }
//        public string AdUnit { get; set; }
//        public string RewardedAdUnit { get; set; }
//    }
//}