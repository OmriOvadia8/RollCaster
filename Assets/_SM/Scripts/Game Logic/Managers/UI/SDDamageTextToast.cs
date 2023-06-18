using UnityEngine;
using TMPro;
using DG.Tweening;
using SD_Core;

namespace SD_UI
{
    public class SDDamageTextToast : SDPoolable
    {
        [SerializeField] TMP_Text amountText;

        [SerializeField] private float tweenTime = 1f;
        [SerializeField] private Vector3 moveAmount = Vector3.up;

        private Tween fadeTween;
        private Tween moveTween;
        private Tween scaleTween;

        public void DamageInit(double amount, Color color, float size)
        {
            amountText.color = color;
            amountText.fontSize = size;
            amountText.text = $"{amount.ToReadableNumber()}";
            DoAnimation();
        }

        public void EarnInit(double amount)
        {
            amountText.text = $"+{amount.ToReadableNumber()}";
            DoAnimation();
        }

        public void SpendInit(double amount)
        {
            amountText.text = $"-{amount.ToReadableNumber()}";
            DoAnimation();
        }

        public void DoAnimation()
        {
            transform.DOKill();
            amountText.color = new Color(amountText.color.r, amountText.color.g, amountText.color.b, 1f); 

            transform.localScale = Vector3.one; 
            Vector3 startPos = transform.localPosition; 

            Vector3 endPos = startPos + new Vector3(0, moveAmount.y, 0); 
            transform.DOLocalMove(endPos, tweenTime).SetEase(Ease.OutSine);

            DOVirtual.Float(1, 0, tweenTime, alpha =>
            {
                var color = amountText.color;
                color.a = alpha;
                amountText.color = color;
            }).OnComplete(ReturnToPool);
        }

        public void HideImmediately()
        {
            fadeTween?.Kill();
            moveTween?.Kill();
            scaleTween?.Kill();

            ReturnToPool();
        }

        public void ReturnToPool()
        {
            Manager.PoolManager.ReturnPoolable(this);
        }

        public override void OnReturnedToPool()
        {
            var tempColor = amountText.color;
            tempColor.a = 1;
            amountText.color = tempColor;
            base.OnReturnedToPool();
        }
    }
}
