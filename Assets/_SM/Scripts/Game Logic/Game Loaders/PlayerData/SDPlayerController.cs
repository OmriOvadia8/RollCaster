using SD_Core;

namespace SD_GameLoad
{
    public class SDPlayerController
    {
        private const float TOTAL_XP_MULTIPLIER_INCREASE = 1.5f;
        private bool hasLeveledUp;
        private SDPlayerData PlayerInfo => SDGameLogic.Instance.Player.PlayerData.PlayerInfo;

        #region Player Leveling

        public void AddPlayerXP(double xp)
        {
            PlayerInfo.CurrentXp += xp;

            if (PlayerInfo.CurrentXp >= PlayerInfo.TotalXpRequired)
            {
                double tempXPRequired = PlayerInfo.TotalXpRequired;
                PlayerLevelUp();
                PlayerInfo.CurrentXp -= tempXPRequired;
            }

            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.UpdateXpUI, null);

            SDGameLogic.Instance.Player.SavePlayerData();
        }

        private void PlayerLevelUp()
        {
            SetLevelUpFlag(true);
            PlayerInfo.Level++;
            UpdateTotalXPRequired();
            EarnAbilityPoints(PointsEvent.LevelUp);
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

        public void EarnAbilityPoints(PointsEvent pointsEvent)
        {
            switch (pointsEvent)
            {
                case PointsEvent.LevelUp:
                    PlayerInfo.AbilityPoints += 10;
                    break;
                case PointsEvent.BossKill:
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

    public enum PointsEvent
    {
        LevelUp,
        BossKill
    }
}
