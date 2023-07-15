using UnityEngine;
using SD_Core;
using System;
using SD_Sound;

namespace SD_UI
{
    /// <summary>
    /// The class is responsible for managing and displaying different types of toasts within the game.
    /// </summary>
    public class SDToastingManager : SDMonoBehaviour
    {
        [SerializeField] private RectTransform damageToastPosition;
        [SerializeField] private RectTransform pointsToastPosition;
        [SerializeField] private RectTransform xPToastPosition;
        [SerializeField] private RectTransform levelUpPosition;
        [SerializeField] private RectTransform failAdPosition;
        [SerializeField] private int frequentTextToastAmount = 20;
        [SerializeField] private int textToastAmount = 3;

        private void OnEnable()
        {
            AddListener(SDEventNames.SpendPointsToast, SpendTextToast);
            AddListener(SDEventNames.EarnPointsToast, EarnTextToast);
            AddListener(SDEventNames.XPToast, XPTextToast);
            AddListener(SDEventNames.LvlUpToast, LevelUpTextToast);
            AddListener(SDEventNames.FailAdToast, FailedAdTextToast);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.SpendPointsToast, SpendTextToast);
            RemoveListener(SDEventNames.EarnPointsToast, EarnTextToast);
            RemoveListener(SDEventNames.XPToast, XPTextToast);
            RemoveListener(SDEventNames.LvlUpToast, LevelUpTextToast);
            RemoveListener(SDEventNames.FailAdToast, FailedAdTextToast);
        }

        private void Start() => MoneyToastPoolInitialization();

        /// <summary>
        /// Display a text toast with the specified amount and poolName.
        /// </summary>
        /// <param name="amount">The value to be displayed on the toast.</param>
        /// <param name="poolName">The name of the pool from which the toast object should be retrieved.</param>
        public void DisplayTextToast(double amount, PoolNames poolName)
        {
            var textToast = (SDTextToast)Manager.PoolManager.GetPoolable(poolName);

            if (textToast == null)
            {
                SDDebug.LogException($"Failed to get a poolable object of type SDTextToast with pool name {poolName}");
                return;
            }

            switch (poolName)
            {
                case PoolNames.DamageAnim1 or PoolNames.DamageAnim2 or PoolNames.DamageAnim3:
                    Vector3 toastPosition = GetRandomToastPosition();
                    textToast.transform.position = toastPosition;
                    textToast.TextDetails("", amount);
                    break;
                case PoolNames.EarnPointsToast:
                    textToast.transform.position = pointsToastPosition.position;
                    textToast.TextDetails("+", amount);
                    break;
                case PoolNames.SpendPointsToast:
                    textToast.transform.position = pointsToastPosition.position;
                    textToast.TextDetails("-", amount);
                    break;
                case PoolNames.LevelUpToast:
                    textToast.transform.position = levelUpPosition.position;
                    textToast.TextDetails("LEVEL UP!", null);
                    InvokeEvent(SDEventNames.PlaySound, SoundEffectType.LevelupAudio);
                    break;
                case PoolNames.XPToast:
                    textToast.transform.position = xPToastPosition.position;
                    textToast.TextDetails("+", amount);
                    break;
                case PoolNames.FailAdToast:
                    textToast.transform.position = failAdPosition.position;
                    textToast.TextDetails("Failed to load ad, Please try again.", null);
                    break;
                default:
                    throw new ArgumentException("Invalid PoolName value");
            }
        }

        private void SpendTextToast(object amount)
        {
            int pointsSpent = (int)amount;
            DisplayTextToast(pointsSpent, PoolNames.SpendPointsToast);
        }

        private void EarnTextToast(object amount)
        {
            int pointsEarned = (int)amount;
            DisplayTextToast(pointsEarned, PoolNames.EarnPointsToast);
        }

        private void XPTextToast(object amount)
        {
            double XPEarned = (double)amount;
            DisplayTextToast(XPEarned, PoolNames.XPToast);
        }

        private void LevelUpTextToast(object obj = null) => DisplayTextToast(0, PoolNames.LevelUpToast);

        private void FailedAdTextToast(object obj = null) => DisplayTextToast(0, PoolNames.FailAdToast);

        private Vector3 GetRandomToastPosition()
        {
            float minX = -1f;
            float maxX = 1f;
            float minY = -1f;
            float maxY = 1f;

            float x = UnityEngine.Random.Range(minX, maxX);
            float y = UnityEngine.Random.Range(minY, maxY);

            return new Vector3(damageToastPosition.position.x + x, damageToastPosition.position.y + y, damageToastPosition.position.z);
        }

        /// <summary>
        /// Initializes all required toast pools with the given parameters.
        /// </summary>
        private void MoneyToastPoolInitialization()
        {
            Manager.PoolManager.InitPool(nameof(PoolNames.DamageAnim1), frequentTextToastAmount, damageToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.DamageAnim2), frequentTextToastAmount, damageToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.DamageAnim3), frequentTextToastAmount, damageToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.EarnPointsToast), textToastAmount, pointsToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.SpendPointsToast), frequentTextToastAmount, pointsToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.XPToast), textToastAmount, xPToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.LevelUpToast), textToastAmount, levelUpPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.FailAdToast), frequentTextToastAmount, failAdPosition);
        }
    }
}
