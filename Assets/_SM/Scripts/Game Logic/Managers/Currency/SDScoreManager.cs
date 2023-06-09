using System.Collections.Generic;
using SD_Core;

namespace SD_Currency
{
    public class SDScoreManager
    {
        public SDPlayerScoreData PlayerScoreData = new();

        public SDScoreManager() => SDManager.Instance.SaveManager.Load((SDPlayerScoreData data) =>
        PlayerScoreData = data ?? new SDPlayerScoreData());

        public bool TryGetScoreByTag(ScoreTags tag, ref double scoreOut)
        {
            if (PlayerScoreData.ScoreByTag.TryGetValue(tag, out var score))
            {
                scoreOut = score;
                return true;
            }

            return false;
        }

        public void SetScoreByTag(ScoreTags tag, double amount = 0)
        {
            SDManager.Instance.EventsManager.InvokeEvent(SDEventNames.OnCurrencySet, (tag, amount));
            PlayerScoreData.ScoreByTag[tag] = amount;

            SDManager.Instance.SaveManager.Save(PlayerScoreData);
        }

        public void ChangeScoreByTagByAmount(ScoreTags tag, double amount = 0)
        {
            if (PlayerScoreData.ScoreByTag.ContainsKey(tag))
            {
                SetScoreByTag(tag, PlayerScoreData.ScoreByTag[tag] + amount);
            }
            else
            {
                SetScoreByTag(tag, amount);
            }
        }

        public bool TryUseScore(ScoreTags scoreTag, double amountToReduce)
        {
            var score = 0D;
            var hasType = TryGetScoreByTag(scoreTag, ref score);
            var hasEnough = false;

            if (hasType)
            {
                hasEnough = amountToReduce <= score;
            }

            if (hasEnough)
            {
                ChangeScoreByTagByAmount(scoreTag, -amountToReduce);
            }
            else
            {
                SDManager.Instance.CrashManager.LogBreadcrumb($"User Doesn't have enough coins of type {scoreTag.ToString()}");
            }

            return hasEnough;
        }
    }

    public class SDPlayerScoreData : ISDSaveData
    {
        public Dictionary<ScoreTags, double> ScoreByTag = new();
    }

    public enum ScoreTags
    {
        GameCurrency,
        PremiumCurrency
    }
}
