using UnityEngine;
using SD_Core;
using System;

namespace SD_UI
{
    public class SDToastingManager : SDMonoBehaviour
    {
        [SerializeField] private RectTransform damageToastPosition;
        [SerializeField] private RectTransform currencyToastPosition;
        [SerializeField] private int textToastAmount = 10;

        public void DisplayMoneyToast(double amount, PoolNames poolName)
        {
            var damageTextToast = (SDDamageTextToast)Manager.PoolManager.GetPoolable(poolName);
            Vector3 toastPosition = GetRandomToastPosition();
            damageTextToast.transform.position = toastPosition;

            switch (poolName)
            {
                case PoolNames.DamageAnim1 or PoolNames.DamageAnim2 or PoolNames.DamageAnim3:
                    damageTextToast.DamageInit(amount);
                    break;
                case PoolNames.EarnCurrencyToast:
                    damageTextToast.EarnInit(amount);
                    break;
                case PoolNames.SpendCurrencyToast:
                    damageTextToast.SpendInit(amount);
                    break;
                default:
                    throw new ArgumentException("Invalid PoolName value");
            }
        }

        private void Start() => MoneyToastPoolInitialization();

        private Vector3 GetRandomToastPosition()
        {
            // Define bounds for random position
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
            Manager.PoolManager.InitPool(nameof(PoolNames.EarnCurrencyToast), textToastAmount, currencyToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.SpendCurrencyToast), textToastAmount, currencyToastPosition);
        }
    }
}
