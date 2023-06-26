using SD_Core;

namespace SD_GameLoad
{
    public class SDPlayerDataManager
    {
        public SDPlayer PlayerData { get; private set; }
        public const string PLAYER_CONFIG = "player";

        public SDPlayerDataManager() => LoadCurrentPlayerData();

        #region Data Loading
        private void LoadCurrentPlayerData()
        {
            SDManager.Instance.SaveManager.Load<SDPlayer>(data =>
            {
                if (data != null)
                {
                    LoadSavedCurrentPlayerData(data);
                    SDDebug.Log("player data loaded from save file");
                }
                else
                {
                    LoadDefaultPlayerData();
                    SDDebug.Log("player data loaded from default file");
                }
            });
        }

        private void LoadSavedCurrentPlayerData(SDPlayer data)
        {
            PlayerData = data;
            if (PlayerData?.PlayerInfo != null)
            {
                SDDebug.Log("Player Data Loaded: LEVEL :" + PlayerData.PlayerInfo.Level);
            }
            else
            {
                SDDebug.LogException("playerdata or playerInfo is null");
            }
        }

        private void OnConfigLoaded(SDPlayer configData)
        {
            PlayerData = configData;
            if (PlayerData?.PlayerInfo != null)
            {
                SDDebug.Log("Player Data Loaded: LEVEL :" + PlayerData.PlayerInfo.Level);
            }
            else
            {
                SDDebug.LogException("playerdata or playerInfo is null");
            }
        }

        private void LoadDefaultPlayerData()
        {
            SDManager.Instance.ConfigManager.GetConfigAsync<SDPlayer>(PLAYER_CONFIG, OnConfigLoaded);
            SDDebug.Log("Default player Data Loaded Successfully");
        }

        public void SavePlayerData() => SDManager.Instance.SaveManager.Save(PlayerData);
        #endregion
    }
}