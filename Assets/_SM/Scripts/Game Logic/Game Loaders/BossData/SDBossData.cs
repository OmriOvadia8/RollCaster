using SD_Core;

namespace SD_GameLoad
{
    public class SDBossData
    {
        public int Level { get; set; }
        public double TotalHp { get; set; }
        public double CurrentHp { get; set; }

        public SDBossData(int level, double totalHp, double currentHp)
        {
            Level = level;
            TotalHp = totalHp;
            CurrentHp = currentHp;
        }
    }

    public class SDCurrentBoss : ISDSaveData
    {
        public SDBossData BossInfo { get; set; }
    }
}