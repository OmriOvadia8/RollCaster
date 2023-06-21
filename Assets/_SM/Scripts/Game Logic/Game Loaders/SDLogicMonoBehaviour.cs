using SD_Core;

namespace SD_GameLoad
{
    public class SDLogicMonoBehaviour : SDMonoBehaviour
    {
        public SDGameLogic GameLogic => SDGameLogic.Instance;
        public SDBossData CurrentBossInfo => GameLogic.CurrentBossData.CurrentBoss.BossInfo;
    }
}