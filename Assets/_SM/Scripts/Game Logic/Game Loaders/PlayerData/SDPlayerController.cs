using SD_Core;

namespace SD_GameLoad
{
    public class SDPlayerController
    {
        private const float TOTAL_XP_MULTIPLIER_INCREASE = 1.5f;

        private SDPlayerData PlayerInfo => SDGameLogic.Instance.Player.PlayerData.PlayerInfo;

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
            PlayerInfo.Level++;
            UpdateTotalXPRequired();
            SDGameLogic.Instance.Player.SavePlayerData();
        }

        private void UpdateTotalXPRequired() => PlayerInfo.TotalXpRequired *= TOTAL_XP_MULTIPLIER_INCREASE;

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
    }
}
