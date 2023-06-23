using SD_Core;

namespace SD_GameLoad
{
    public class SDBossDataManager
    {
        public SDCurrentBoss CurrentBoss { get; private set; }
        public const string BOSS_CONFIG = "current_boss";

        public SDBossDataManager() => LoadCurrentBossData();

        #region Data Loading
        private void LoadCurrentBossData()
        {
            SDManager.Instance.SaveManager.Load<SDCurrentBoss>(data =>
            {
                if (data != null)
                {
                    LoadSavedCurrentBossData(data);
                    SDDebug.Log("Boss data loaded from save file");
                }
                else
                {
                    LoadDefaultBossData();
                    SDDebug.Log("Boss data loaded from default file");
                }
            });
        }

        private void LoadSavedCurrentBossData(SDCurrentBoss data)
        {
            CurrentBoss = data;
            if (CurrentBoss?.BossInfo != null)
            {
                SDDebug.Log("Boss Data Loaded: HP :" + CurrentBoss.BossInfo.TotalHp);
            }
            else
            {
                SDDebug.LogException("CurrentBoss or BossInfo is null");
            }

        }

        private void OnConfigLoaded(SDCurrentBoss configData)
        {
            CurrentBoss = configData;
            if (CurrentBoss?.BossInfo != null)
            {
                SDDebug.Log("Boss Data Loaded: HP :" + CurrentBoss.BossInfo.TotalHp);
            }
            else
            {
                SDDebug.LogException("currentboss or BossInfo is null");
            }

        }

        private void LoadDefaultBossData()
        {
            SDManager.Instance.ConfigManager.GetConfigAsync<SDCurrentBoss>(BOSS_CONFIG, OnConfigLoaded);
            SDDebug.Log("Default boss Data Loaded Successfully");
        }

        public void SaveCurrentBossData() => SDManager.Instance.SaveManager.Save(CurrentBoss);
        #endregion
    }
}