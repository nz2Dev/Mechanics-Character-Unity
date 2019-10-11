using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePreparingTransitionBehaviour : StateMachineBehaviour {

    public float MinReverseTime = 0.1f;
    // public float MaxStartAtTime = 0.9f;
    
    private bool reverse;

    public void Trigger() {
        reverse = true;
    }

    public void ResetFlag() {
        reverse = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (reverse && stateInfo.normalizedTime > MinReverseTime) {
            reverse = false;
//            animator.PlayInFixedTime("ReversePreparing", layerIndex, Mathf.Min(1 - stateInfo.normalizedTime, MaxStartAtTime) * stateInfo.length);
            animator.PlayInFixedTime("ReversePreparing", layerIndex, (1 - stateInfo.normalizedTime) * stateInfo.length);
        }
    }

}