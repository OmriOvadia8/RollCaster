using System;
using System.Collections.Generic;

namespace SM_Core
{
    public class SMEventsManager
    {
        private readonly Dictionary<SMEventNames, List<Action<object>>> activeListeners = new();

        public void AddListener(SMEventNames eventName, Action<object> onGameStart)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Add(onGameStart);
                return;
            }

            activeListeners.Add(eventName, new List<Action<object>> { onGameStart });
        }

        public void RemoveListener(SMEventNames eventName, Action<object> onGameStart)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Remove(onGameStart);

                if (listOfEvents.Count <= 0)
                {
                    activeListeners.Remove(eventName);
                }
            }
        }

        public void InvokeEvent(SMEventNames eventName, object obj)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                foreach (var action in listOfEvents)
                {
                    action.Invoke(obj);
                }
            }
        }
    }

    public enum SMEventNames
    {
        OnCurrencySet,
        OnUpgradeTextUpdate,
        MoneyToastOnCook,
        MoneyToastOnAutoCook,
        OnUpgradeMoneySpentToast,
        OnHireMoneySpentToast,
        OnHiredTextUpdate,
        OnPause,
        OfflineTimeRefreshed,
        OnLearnRecipe,
        OnLearnRecipeSpentToast,
        CurrencyUpdateUI,
        OnPremCurrencySet,
        PremCurrencyUpdateUI,
        DeviceAppearAnimation,
        AddCurrencyUpdate,
        StartBakerCooking,
        BuyButtonsCheck,
        CookButtonAlphaOn,
        CookButtonAlphaOff,
        BakerParticles,
        LearnParticles,
        FoodBarReveal,
        FoodBarLocked,
        UpgradeParticles,
        CookParticles,
        CookFoodButtonCheck,
        CheckCookedAchievement,
        CheckHiredAchievement,
        CurrentMakeFoodAchievementStatus,
        CurrentHireBakerAchievementStatus,
        AddStarsUpdate,
        MakeFoodProgressUpdate,
        HireBakerProgressUpdate,
        CheckBuySkinButtonUI,
        BuySkinButtonVisibility,
        CheckBuyTimeWrapButtonsUI,
        PlaySound,
        AchievementPing,
        BakerPing,
        SlimeAction,
        TimeWrapCoinsText
    }
}