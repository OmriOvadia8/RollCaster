using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD_Core;

namespace SD_UI
{
    public class SDBossAnimationsController : SDMonoBehaviour
    {
        [SerializeField] Animator bossAnimator;

        private bool isHurt;
        private bool isDead;

        private void OnEnable()
        {
            AddListener(SDEventNames.HurtBoss, HurtBoss);
            AddListener(SDEventNames.KillBoss, KillBoss);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.HurtBoss, HurtBoss);
            RemoveListener(SDEventNames.KillBoss, KillBoss);
        }

        public void HurtBoss(object obj = null)
        {
            isHurt = true;
            bossAnimator.SetBool("isHurt", isHurt);

            StartCoroutine(RecoverBoss(0.35f));
        }

        public void KillBoss(object obj = null)
        {
            isDead = true;
            bossAnimator.SetBool("isDead", isDead);
        }

        private IEnumerator RecoverBoss(float timeToRecover)
        {
            yield return new WaitForSeconds(timeToRecover);

            isHurt = false;
            bossAnimator.SetBool("isHurt", isHurt);
        }
    }
}