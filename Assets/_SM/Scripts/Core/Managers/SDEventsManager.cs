using System;
using System.Collections.Generic;

namespace SD_Core
{
    public class SDEventsManager
    {
        private readonly Dictionary<SDEventNames, List<Action<object>>> activeListeners = new();

        public void AddListener(SDEventNames eventName, Action<object> onGameStart)
        {
            if (activeListeners.TryGetValue(eventName, out var listOfEvents))
            {
                listOfEvents.Add(onGameStart);
                return;
            }

            activeListeners.Add(eventName, new List<Action<object>> { onGameStart });
        }

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