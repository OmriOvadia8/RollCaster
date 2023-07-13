using SD_Core;

namespace SD_GameLoad
{
    /// <summary>
    /// Represents the data related to a boss.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the SDBossData class.
        /// </summary>
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