using SD_Core;

namespace SD_GameLoad
{
    public class SDPlayerData
    {
        public int Level { get; set; }
        public double TotalXpRequired { get; set; }
        public double CurrentXp { get; set; }
        public int MaxRolls { get; set; }
        public int CurrentRolls { get; set; }

        public SDPlayerData(int level, double totalXP, double currentXP, int maxRolls, int currentRolls)
        {
            Level = level;
            TotalXpRequired = totalXP;
            CurrentXp = currentXP;
            MaxRolls = maxRolls;
            CurrentRolls = currentRolls;
        }
    }

    public class SDPlayer : ISDSaveData
    {
        public SDPlayerData PlayerInfo { get; set; }
    }
}