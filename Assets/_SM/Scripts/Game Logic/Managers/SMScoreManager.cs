using System.Collections.Generic;
using SM_Core;

namespace SM_GameManagers
{
    public class SMScoreManager
    {
        public SMPlayerScoreData PlayerScoreData = new();

        public SMScoreManager()
        {
            SMManager.Instance.SaveManager.Load<SMPlayerScoreData>(delegate (SMPlayerScoreData data)
            {
                PlayerScoreData = data ?? new SMPlayerScoreData();
            });
        }

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
            SMManager.Instance.EventsManager.InvokeEvent(SMEventNames.OnCurrencySet, (tag, amount));
            PlayerScoreData.ScoreByTag[tag] = amount;

            SMManager.Instance.SaveManager.Save(PlayerScoreData);
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
                SMManager.Instance.CrashManager.LogBreadcrumb($"User Doesn't have enough coins of type {scoreTag.ToString()}");
            }

            return hasEnough;
        }
    }

    public class SMPlayerScoreData : ISMSaveData
    {
        public Dictionary<ScoreTags, double> ScoreByTag = new();
    }

    public enum ScoreTags
    {
        GameCurrency,
        PremiumCurrency
    }
}
