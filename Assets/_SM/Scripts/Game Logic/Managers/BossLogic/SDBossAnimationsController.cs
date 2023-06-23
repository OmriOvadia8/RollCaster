using System.Collections;
using SD_GameLoad;
using UnityEngine;
using SD_Core;
using System;
using System.Collections.Generic;

namespace SD_Boss
{
    public class SDBossAnimationsController : SDLogicMonoBehaviour
    {
        [SerializeField] BossComponents[] bossComponents;

        private bool isHurt;
        private bool isDead;
        private List<int> bossIndices = new List<int> { 0, 1};
        private int specialBossIndex = 2;
        private const int SPECIAL_BOSS_LEVEL = 10;

        private void OnEnable()
        {
            AddListener(SDEventNames.HurtBoss, HurtBoss);
            AddListener(SDEventNames.KillBoss, KillBoss);
            AddListener(SDEventNames.SpawnBoss, SpawnBoss);
        }

        private void OnDisable()
        {
            RemoveListener(SDEventNames.HurtBoss, HurtBoss);
            RemoveListener(SDEventNames.KillBoss, KillBoss);
            RemoveListener(SDEventNames.SpawnBoss, SpawnBoss);
        }

        private void Start() => SpawnBoss();

        public void SpawnBoss(object obj = null) => StartCoroutine(SpawnBossWithDelay(1.4f));

        public void HurtBoss(object obj = null)
        {
            isHurt = true;
            int index = CurrentBossInfo.Index;
            bossComponents[index].animator.SetBool(nameof(BossAnimations.IsHurt), isHurt);

            StartCoroutine(RecoverBoss(0.35f, index));
        }

        public void KillBoss(object obj = null)
        {
            isDead = true;
            int index = CurrentBossInfo.Index;
            bossComponents[index].animator.SetBool(nameof(BossAnimations.IsDead), isDead);
            StartCoroutine(HideBoss(1.35f, index));
        }

        private IEnumerator RecoverBoss(float timeToRecover, int index)
        {
            yield return new WaitForSeconds(timeToRecover);

            isHurt = false;
            bossComponents[index].animator.SetBool(nameof(BossAnimations.IsHurt), isHurt);
        }

        private IEnumerator SpawnBossWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (CurrentBossInfo != null)
            {
                int bossIndex = GetBossIndex();
                CurrentBossInfo.Index = bossIndex;
                bossComponents[bossIndex].gameObject.SetActive(true);
                CurrentBossInfo.IsAlive = true;
                InvokeEvent(SDEventNames.SpinEnable, true);
            }
        }

        private IEnumerator HideBoss(float timeToHide, int index)
        {
            CurrentBossInfo.IsAlive = false;
            InvokeEvent(SDEventNames.SpinEnable, false);

            yield return new WaitForSeconds(timeToHide);
            bossComponents[index].gameObject.SetActive(false);
        }

        private int GetBossIndex()
        {
            if (CurrentBossInfo.Level % SPECIAL_BOSS_LEVEL == 0)
            {
                return specialBossIndex;
            }

            return bossIndices[(CurrentBossInfo.Level - 1) % bossIndices.Count];
        }

    }

    [Serializable]
    public class BossComponents
    {
        public GameObject gameObject;
        public Animator animator;
    }

    [Serializable]
    public enum BossAnimations
    {
        IsHurt,
        IsDead,
    }

}
