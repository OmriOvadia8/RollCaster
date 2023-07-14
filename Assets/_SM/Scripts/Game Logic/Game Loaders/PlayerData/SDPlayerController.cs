using SD_Core;

namespace SD_GameLoad
{
    /// <summary>
    /// Handles player controls and actions.
    /// </summary>
    public class SDPlayerController
    {
        private const double TOTAL_XP_MULTIPLIER_INCREASE = 1.18;
        private const int NORMAL_KILL_POINTS = 2;
        private const int BONUS_GAINED_LEVEL = 5;
        private bool hasLeveledUp;
        private SDPlayerData PlayerInfo => SDGameLogic.Instance.Player.PlayerData.PlayerInfo;

        #region Player Leveling
        /// <summary>
        /// Adds experience points to the player's total.
        /// </summary>
        /// <param name="xp">The amount of experience points to add.</param>
        public void AddPlayerXP(double xp)
        {
            PlayerInfo.CurrentXp += xp;
            SDDebug.Log($"Before Level Up: CurrentXp = {PlayerInfo.CurrentXp}, TotalXpRequired = {PlayerInfo.TotalXpRequired}");

            if (PlayerInfo.CurrentXp >= PlayerInfo.TotalXpRequired)
            {
                double tempXPRequired = PlayerInfo.TotalXpRequired;
                PlayerLevelUp();
                PlayerInfo.CurrentXp -= tempXPRequired;
                SDDebug.Log($"After Level Up: CurrentXp = {PlayerInfo.CurrentXp}, TotalXpRequired = {PlayerInfo.TotalXpRequired}, tempXPRequired = {tempXPRequired}");
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.XPToast, xp);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateXpUI, null);
            SDGameLogic.Instance.Player.SavePlayerData();
        }

        private void PlayerLevelUp()
        {
            SetLevelUpFlag(true);
            PlayerInfo.Level++;
            UpdateTotalXPRequired();
            EarnAbilityPoints(PointsEarnTypes.LevelUp);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.LvlUpToast, null);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.CheckUnlockAbility, PlayerInfo.Level);
            SDGameLogic.Instance.Player.SavePlayerData();
        }

        private void UpdateTotalXPRequired() => PlayerInfo.TotalXpRequired *= TOTAL_XP_MULTIPLIER_INCREASE;

        public void SetLevelUpFlag(bool value) => hasLeveledUp = value;

        public bool GetLevelUpFlag()
        {
            return hasLeveledUp;
        }
        #endregion

        #region Player Rolls
        public void DecreaseRoll()
        {
            if (PlayerInfo.ExtraRolls > 0)
            {
                PlayerInfo.ExtraRolls--;
            }
            else if (GetCurrentRollsAmount() > 0)
            {
                PlayerInfo.CurrentRolls--;
                if (!PlayerInfo.IsRollRegenOn)
                {
                    SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.StartRollsRegeneration, null);
                }
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateRollsUI, null);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.CheckRollsForSpin, null);
            SDGameLogic.Instance.Player.SavePlayerData();
        }

        public void IncreaseRoll(int rolls)
        {
            int neededRolls = PlayerInfo.MaxRolls - PlayerInfo.CurrentRolls;

            if (rolls <= neededRolls)
            {
                PlayerInfo.CurrentRolls += rolls;
            }
            else
            {
                PlayerInfo.CurrentRolls = PlayerInfo.MaxRolls;
                PlayerInfo.ExtraRolls += rolls - neededRolls;
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateRollsUI, null);
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.CheckRollsForSpin, null);
            SDGameLogic.Instance.Player.SavePlayerData();
        }

        public int GetCurrentRollsAmount()
        {
            return PlayerInfo.CurrentRolls;
        }

        public int GetMaxRollsAmount()
        {
            return PlayerInfo.MaxRolls;
        }

        public bool IsRegenOn()
        {
            return PlayerInfo.IsRollRegenOn;
        }

        #endregion

        #region Player Points

        /// <summary>
        /// Awards ability points to the player based on the specified event.
        /// </summary>
        /// <param name="pointsEvent">The event that triggered the awarding of points.</param>
        public void EarnAbilityPoints(PointsEarnTypes pointsEvent)
        {
            int levelGroup = PlayerInfo.Level / BONUS_GAINED_LEVEL;
            int levelUpPoints = BONUS_GAINED_LEVEL + (BONUS_GAINED_LEVEL * levelGroup);
            int normalKillPoints = NORMAL_KILL_POINTS + (NORMAL_KILL_POINTS * levelGroup);

            switch (pointsEvent)
            {
                case PointsEarnTypes.LevelUp:
                    PlayerInfo.AbilityPoints += levelUpPoints;
                    SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.EarnPointsToast, levelUpPoints);
                    break;
                case PointsEarnTypes.BossKill:
                    PlayerInfo.AbilityPoints += normalKillPoints;
                    SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.EarnPointsToast, normalKillPoints);
                    break;
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateAbilityPtsUI, null);
            SDGameLogic.Instance.Player.SavePlayerData();
        }

        public void SpendAbilityPoints(int abilityPoints)
        {
            if (PlayerInfo.AbilityPoints >= abilityPoints)
            {
                PlayerInfo.AbilityPoints -= abilityPoints;
                SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateAbilityPtsUI, null);
                SDGameLogic.Instance.Player.SavePlayerData();
            }
        }

        public int GetAbilityPointsAmount()
        {
            return PlayerInfo.AbilityPoints;
        }

        public bool CanSpendAbilityPoints(int abilityPoints)
        {
            return PlayerInfo.AbilityPoints >= abilityPoints;
        }

        #endregion
    }

    public enum PointsEarnTypes
    {
        LevelUp,
        BossKill
    }
}
