using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD_UI
{
    public class AdjustScaleBehaviour : StateMachineBehaviour
    {
        public Vector2 sizeAdjustments;
        public RectTransform animationHolder;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animationHolder = animator.GetComponent<RectTransform>();
            animationHolder.sizeDelta = sizeAdjustments;
        }
    }
}