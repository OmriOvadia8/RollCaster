using UnityEngine;
using SD_Core;
using System;

namespace SD_UI
{
    public class SDToastingManager : SDMonoBehaviour
    {
        [SerializeField] private RectTransform damageToastPosition;
        [SerializeField] private RectTransform pointsToastPosition;
        [SerializeField] private RectTransform levelupToastPosition;
        [SerializeField] private RectTransform XPToastPosition;
        [SerializeField] private int textToastAmount = 10;

        public void DisplayTextToast(double amount, PoolNames poolName)
        {
            var textToast = (SDTextToast)Manager.PoolManager.GetPoolable(poolName);

            switch (poolName)
            {
                case PoolNames.DamageAnim1 or PoolNames.DamageAnim2 or PoolNames.DamageAnim3:
                    Vector3 toastPosition = GetRandomToastPosition();
                    textToast.transform.position = toastPosition;
                    textToast.TextDetails("", amount);
                    break;
                case PoolNames.EarnPointsToast:
                    textToast.transform.position = pointsToastPosition.position;
                    textToast.TextDetails("+" , amount);
                    break;
                case PoolNames.SpendPointsToast:
                    textToast.transform.position = pointsToastPosition.position;
                    textToast.TextDetails("-" , amount);
                    break;
                case PoolNames.LevelUpToast:
                    textToast.transform.position = levelupToastPosition.position;
                    textToast.TextDetails("LEVEL UP!", null);
                    break;
                case PoolNames.XPToast:
                    textToast.transform.position = XPToastPosition.position;
                    textToast.TextDetails("+", amount);
                    break;
                default:
                    throw new ArgumentException("Invalid PoolName value");
            }
        }

        private void Start() => MoneyToastPoolInitialization();

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

        private void MoneyToastPoolInitialization()
        {
            Manager.PoolManager.InitPool(nameof(PoolNames.DamageAnim1), textToastAmount, damageToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.DamageAnim2), textToastAmount, damageToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.DamageAnim3), textToastAmount, damageToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.EarnPointsToast), textToastAmount, pointsToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.SpendPointsToast), textToastAmount, pointsToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.XPToast), textToastAmount, XPToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.LevelUpToast), textToastAmount, levelupToastPosition);
        }
    }
}
