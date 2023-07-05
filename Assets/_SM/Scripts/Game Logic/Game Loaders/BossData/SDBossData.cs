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
        public int HPRegenDuration { get; set; }
        public int CurrentHPRegenDuration { get; set; }
        public bool IsHPRegenOn { get; set; }
        public bool IsAlive { get; set; }

        public SDBossData(int level, double totalHp, double currentHp, int index, double xP, double specialBossLevel, int hPRegenDuration, int currentHPRegenDuration, bool isHPRegenOn)
        {
            Index = index;
            Level = level;
            TotalHp = totalHp;
            CurrentHp = currentHp;
            XP = xP;
            SpecialBossLevel = specialBossLevel;
            HPRegenDuration = hPRegenDuration;
            CurrentHPRegenDuration = currentHPRegenDuration;
            IsHPRegenOn = isHPRegenOn;
        }
    }

    public class SDCurrentBoss : ISDSaveData
    {
        public SDBossData BossInfo { get; set; }
    }
}