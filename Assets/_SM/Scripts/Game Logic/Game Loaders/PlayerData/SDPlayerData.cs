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
        public int RollRegenDuration { get; set; }
        public int RollRegenCurrentDuration { get; set; }
        public bool IsRollRegenOn { get; set; }
        public int ExtraRolls { get; set; }

        public SDPlayerData(int level, double totalXP, double currentXP, int maxRolls, int currentRolls, int abilityPoints, int rollRegenDuration, int rollRegenCurrentDuration, bool isRollRegenOn, int extraRolls)
        {
            Level = level;
            TotalXpRequired = totalXP;
            CurrentXp = currentXP;
            MaxRolls = maxRolls;
            CurrentRolls = currentRolls;
            AbilityPoints = abilityPoints;
            RollRegenDuration = rollRegenDuration;
            RollRegenCurrentDuration = rollRegenCurrentDuration;
            IsRollRegenOn = isRollRegenOn;
            ExtraRolls = extraRolls;
        }
    }

    public class SDPlayer : ISDSaveData
    {
        public SDPlayerData PlayerInfo { get; set; }
    }
}