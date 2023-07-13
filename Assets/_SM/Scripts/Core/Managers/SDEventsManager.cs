using System;
using System.Collections.Generic;

namespace SD_Core
{
    /// <summary>
    /// Manages all in-game events.
    /// Uses a dictionary of delegates to handle events and their respective actions.
    /// </summary>
    public class SDEventsManager
    {
        private readonly Dictionary<SDEventNames, List<Action<object>>> activeListeners = new();

        /// <summary>
        /// Adds a new listener to the specified event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="onGameStart">The action to perform when the event is triggered.</param>

        public void AddListener(SDEventNames eventName, Action<object> onGameStart)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Add(onGameStart);
                return;
            }

            activeListeners.Add(eventName, new List<Action<object>> { onGameStart });
        }

        /// <summary>
        /// Removes a listener from the specified event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="onGameStart">The action to be removed from the event's listeners.</param>
        public void RemoveListener(SDEventNames eventName, Action<object> onGameStart)
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

        /// <summary>
        /// Invokes the specified event, triggering all its associated actions.
        /// </summary>
        /// <param name="eventName">The name of the event to be invoked.</param>
        /// <param name="obj">The object to pass to the action.</param>
        public void InvokeEvent(SDEventNames eventName, object obj)
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

    public enum SDEventNames
    {
        OnPause,
        OfflineTimeRefreshed,
        OnCurrencySet,
        OnAbilityRolled,
        AbilityAnim1,
        AbilityAnim2,
        AbilityAnim3,
        AbilityAnim,
        CastDamage,
        OnDamageToast,
        HurtBoss,
        KillBoss,
        SpawnBoss,
        SpinEnable,
        UpdateBossLevelUI,
        UpdateHealthUI,
        UpdateXpUI,
        UpdateRollsUI,
        BossCrownVisibility,
        UpdateAbilityPtsUI,
        UpdateAbilityUpgradeButtons,
        UpdateAbilityUnlockedUI,
        UpdateAbilityUpgradeUI,
        CheckUnlockAbility,
        UpdateAllUpgradesButtons,
        CheckRollsForSpin,
        SpendPointsToast,
        EarnPointsToast,
        XPToast,
        LvlUpToast,
        StartRollsRegeneration,
        StartHPRegeneration,
        StopHPRegen,
        BossQuest,
        UpdateQuest,
        QuestToast,
        NewSkillToast,
        FailAdToast,
        PlaySound,
    }
}