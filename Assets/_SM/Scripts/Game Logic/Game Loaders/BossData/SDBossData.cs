using SD_Core;

namespace SD_GameLoad
{
    public class SDBossData
    {
        public int Index { get; set; }
        public int Level { get; set; }
        public double TotalHp { get; set; }
        public double CurrentHp { get; set; }
        public double XP { get; set; }
        public double SpecialBossLevel { get; set; }
        public bool IsAlive { get; set; }

        public SDBossData(int level, double totalHp, double currentHp, int index, double xP, double specialBossLevel)
        {
            Index = index;
            Level = level;
            TotalHp = totalHp;
            CurrentHp = currentHp;
            XP = xP;
            SpecialBossLevel = specialBossLevel;
        }
    }

    public class SDCurrentBoss : ISDSaveData
    {
        public SDBossData BossInfo { get; set; }
    }
}