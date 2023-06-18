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

        private void OnEnable()
        {
            Manager.EventsManager.AddListener(SDEventNames.OnAbilityRolled, DisplayTextToast);
        }

        private void OnDisable()
        {
            Manager.EventsManager.RemoveListener(SDEventNames.OnAbilityRolled, DisplayTextToast);
        }

        private void DisplayTextToast(object obj)
        {
            DamageEventDetails details = (DamageEventDetails)obj;
            InsertToastDetails(details.DamageAmount, PoolNames.DamageToast, details.TextColor, details.TextSize);
        }

        private void Start() => MoneyToastPoolInitialization();

        public void InsertToastDetails(double amountText, PoolNames poolName, Color color, float size)
        {
            var damageTextToast = (SDDamageTextToast)Manager.PoolManager.GetPoolable(poolName);
            Vector3 toastPosition = GetRandomToastPosition();
            damageTextToast.transform.position = toastPosition;

            switch (poolName)
            {
                case PoolNames.DamageToast:
                    damageTextToast.DamageInit(amountText, color, size);
                    break;
                case PoolNames.EarnCurrencyToast:
                    damageTextToast.EarnInit(amountText);
                    break;
                case PoolNames.SpendCurrencyToast:
                    damageTextToast.SpendInit(amountText);
                    break;
                default:
                    throw new ArgumentException("Invalid PoolName value");
            }
        }


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
            Manager.PoolManager.InitPool(nameof(PoolNames.DamageToast), textToastAmount, damageToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.EarnCurrencyToast), textToastAmount, currencyToastPosition);
            Manager.PoolManager.InitPool(nameof(PoolNames.SpendCurrencyToast), textToastAmount, currencyToastPosition);
        }
    }
}
