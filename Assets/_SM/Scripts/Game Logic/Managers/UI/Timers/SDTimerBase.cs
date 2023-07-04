//using DG.Tweening;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SD_Core;
//using SD_GameLoad;
//using TMPro;

//namespace SD_UI
//{
//    public class SDTimerBase : SDLogicMonoBehaviour
//    {

//        protected void SetCookingUI(ref int timeLeft, int rollTime, TMP_Text timeText)
//        {
//            timeLeft = rollTime;
//            timeText.text = SDExtension.GetFormattedTimeSpan(timeLeft);
//        }

//        protected void SetCookingUIAfterPause(int cookingTime, int timeLeft,
//                                    SDTweenTypes tweenType, TMP_Text timeText, SDTimerTypes timerType)
//        {
//            switch (timerType)
//            {
//                case SDTimerTypes.RollRegen:
//                    GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration = timeLeft;
//                    break;

//                //case SDTimerTypes.BossRegen:
//                //    foodData.RemainingBakerCookingTime = timeLeft;
//                //    break;

//                default:
//                    throw new ArgumentException("Invalid SDTimerTypes value");
//            }

//            timeText.text = SDExtension.GetFormattedTimeSpan(timeLeft);

//        }

//        protected void UpdateCookingUIOnTimerTick(Func<int> getTimeLeft, Tween timerTween, TimeSpan remainingTime, TMP_Text timeText, SDTimerTypes timerType)
//        {
//            int previousTime = getTimeLeft();
//            timerTween.OnUpdate(() =>
//            {
//                int timeLeft = getTimeLeft();
//                remainingTime = TimeSpan.FromSeconds(timeLeft);

//                if (previousTime != timeLeft)
//                {
//                    UpdateRemainingCookingTime(timerType, timeLeft);
//                    previousTime = timeLeft;
//                }
//                timeText.text = SDExtension.FormatTimeSpan(remainingTime);
//            });
//        }

//        protected Tween CookingTweenTimer(int index, float duration, SDTweenTypes tweenType, Action<int> onUpdate)
//        {
//            return DOTween.To(() => duration, x => onUpdate(x), DBCookingUIManager.MIN_VALUE, duration).SetEase(Ease.Linear).SetId(tweenType + index);
//        }

//        private void UpdateRemainingCookingTime(SDTimerTypes cookingType, int time)
//        {
//            switch (cookingType)
//            {
//                case SDTimerTypes.RollRegen:
//                    GameLogic.Player.PlayerData.PlayerInfo.RollRegenCurrentDuration = time;
//                    break;

//                //case CookingType.BakerCooking:
//                //    foodData.RemainingBakerCookingTime = time;
//                //    break;

//                default:
//                    throw new ArgumentException("Invalid CookingType value");
//            }

//            GameLogic.Player.SavePlayerData();
//        }
//    }

//    public enum SDTimerTypes
//    {
//        RollRegen,
//        BossRegen
//    }

//    public enum SDTweenTypes
//    {
//        Roll,
//        Boss
//    }
//}