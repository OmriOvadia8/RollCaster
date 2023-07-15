using UnityEngine;
using TMPro;
using DG.Tweening;
using SD_Core;

namespace SD_UI
{
    /// <summary>
    /// This class is responsible for handling the behavior and functionality of a text toast within the game.
    /// </summary>
    public class SDTextToast : SDPoolable
    {
        [SerializeField] TMP_Text amountText;
        [SerializeField] private float tweenTime = 1f;
        [SerializeField] private Vector3 moveAmount = Vector3.up;

        /// <summary>
        /// Set the text details and animate the text toast.
        /// </summary>
        /// <param name="suffix">The string that precedes the amount.</param>
        /// <param name="amount">The amount to be displayed on the toast. If null, only the suffix is displayed.</param>
        public void TextDetails(string suffix, double? amount)
        {
            if (amount.HasValue)
            {
                amountText.text = suffix + amount.Value.ToReadableNumber();
            }
            else
            {
                amountText.text = suffix;
            }

            DoAnimation();
        }

        /// <summary>
        /// Executes the animation of the text toast.
        /// </summary>
        private void DoAnimation()
        {
            SDDebug.Log("DoAnimation called");
            transform.DOKill();
            amountText.color = new Color(amountText.color.r, amountText.color.g, amountText.color.b, 1f);

            transform.localScale = Vector3.one;
            Vector3 startPos = transform.localPosition;

            Vector3 endPos = startPos + moveAmount;
            transform.DOLocalMove(endPos, tweenTime).SetEase(Ease.OutSine);

            DOVirtual.Float(1, 0, tweenTime, alpha =>
            {
                var color = amountText.color;
                color.a = alpha;
                amountText.color = color;
            })
            .OnStart(() => SDDebug.Log("Tween started"))
            .OnComplete(ReturnToPool);
        }

        private void ReturnToPool()
        {
            SDDebug.Log("ReturnToPool called");
            Manager.PoolManager.ReturnPoolable(this);
        }

        /// <summary>
        /// Invoked when the object is returned to the pool. Restores the original color of the text.
        /// </summary>
        public override void OnReturnedToPool()
        {
            var tempColor = amountText.color;
            tempColor.a = 1;
            amountText.color = tempColor;
            base.OnReturnedToPool();
        }
    }
}
