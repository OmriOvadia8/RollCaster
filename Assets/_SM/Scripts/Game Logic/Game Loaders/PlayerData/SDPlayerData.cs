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
        public int AbilityPoints { get; set; }

        public SDPlayerData(int level, double totalXP, double currentXP, int maxRolls, int currentRolls, int abilityPoints)
        {
            Level = level;
            TotalXpRequired = totalXP;
            CurrentXp = currentXP;
            MaxRolls = maxRolls;
            CurrentRolls = currentRolls;
            AbilityPoints = abilityPoints;
        }
    }

    public class SDPlayer : ISDSaveData
    {
        public SDPlayerData PlayerInfo { get; set; }
    }
}