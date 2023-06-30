using SD_Core;

namespace SD_GameLoad
{
    public class SDPlayerController
    {
        private const double TOTAL_XP_MULTIPLIER_INCREASE = 1.2;
        private bool hasLeveledUp;
        private SDPlayerData PlayerInfo => SDGameLogic.Instance.Player.PlayerData.PlayerInfo;

        #region Player Leveling
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

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateXpUI, null);
            SDGameLogic.Instance.Player.SavePlayerData();
        }

        private void PlayerLevelUp()
        {
            SetLevelUpFlag(true);
            PlayerInfo.Level++;
            UpdateTotalXPRequired();
            EarnAbilityPoints(PointsEarnTypes.LevelUp);
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
            if (PlayerInfo.CurrentRolls > 0)
            {
                PlayerInfo.CurrentRolls--;
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateRollsUI, null);

            SDGameLogic.Instance.Player.SavePlayerData();
        }

        public void IncreaseRoll()
        {
            if (PlayerInfo.CurrentRolls < PlayerInfo.MaxRolls)
            {
                PlayerInfo.CurrentRolls++;
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateRollsUI, null);

            SDGameLogic.Instance.Player.SavePlayerData();
        }

        #endregion

        #region Player Points

        public void EarnAbilityPoints(PointsEarnTypes pointsEvent)
        {
            switch (pointsEvent)
            {
                case PointsEarnTypes.LevelUp:
                    PlayerInfo.AbilityPoints += 10;
                    break;
                case PointsEarnTypes.BossKill:
                    int points = UnityEngine.Random.Range(1, 4);
                    PlayerInfo.AbilityPoints += points;
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
            else
            {
                SDDebug.Log("Player has not enough points");
            }
        }

        public int GetAbilityPointsAmount()
        {
            return PlayerInfo.AbilityPoints;
        }

        #endregion

    }

    public enum PointsEarnTypes
    {
        LevelUp,
        BossKill
    }
}
