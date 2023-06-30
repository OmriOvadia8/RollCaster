using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SD_Core
{ 
    public static class SDExtension
    {
        public static void WaitForAnimationComplete(this Animator animator, SDMonoBehaviour monoBehaviour, Action onComplete)
        {
            monoBehaviour.WaitForFrame(() =>
            {
                var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length > 0)
                {
                    var animationTime = clipInfo[0].clip.length;
                    monoBehaviour.WaitForSeconds(animationTime, delegate
                    {
                        onComplete?.Invoke();
                    });
                }
                else
                {
                    onComplete?.Invoke();
                }

            });
        }
         
        public static int HoursToSeconds(this int hours)
        {
            return hours * 60 * 60;
        }

        public static int HoursToMin(this int hours)
        {
            return hours * 60;
        }

        public static int MinToSeconds(this int min)
        {
            return min * 60;
        }

        public static string GetRandomString(this string[] stringArray)
        {
            return stringArray[Random.Range(0, stringArray.Length)];
        }

        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            string formatString = (timeSpan.TotalHours >= 1) ? @"hh\:mm\:ss" : @"mm\:ss";
            return timeSpan.ToString(formatString);
        }

        //public static void WatchAd()
        //{
        //    SDManager.Instance.AdsManager.ShowAd();
        //}

        //public static void WatchRewardedAd(Action<bool> onShowAdComplete)
        //{
        //    SDManager.Instance.AdsManager.ShowRewardedAd(onShowAdComplete);
        //}


        public static string GetFormattedTimeSpan(int seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            if (timeSpan.TotalHours >= 1)
            {
                return string.Format("{0}:{1:mm}:{1:ss}", (int)timeSpan.TotalHours, timeSpan);
            }
            else
            {
                return timeSpan.ToString("mm':'ss");
            }
        }

        public static string GetFormattedNumber(this double score)
        {
            if (score < 1000000)
            {
                return string.Format("{0:N0}", score);
            }
            else
            {
                return score.ToReadableNumber();
            }
        }


        public static string ToReadableNumber(this double score, int decimalPlaces = 2)
        {
            string result;
            string[] scoreNames = new string[] { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
            int i;

            for (i = 0; i < scoreNames.Length; i++)
            {
                if (score < 1000)
                {
                    break;
                }
                else
                {
                    score /= 1000;
                }
            }

            if (i == 0)
            {
                result = $"{Math.Floor(score)}";
            }
            else
            {
                string format = "F" + decimalPlaces.ToString();
                result = score.ToString(format);

                result = result.IndexOf('.') < 0 ? result : result.TrimEnd('0').TrimEnd('.');
            }

            return $"{result}{scoreNames[i]}";
        }

    }
}