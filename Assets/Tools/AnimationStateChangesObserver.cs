using System;
using UnityEngine;

public class AnimationStateChangesObserver : StateMachineBehaviour {

    public event Action<Animator, AnimatorStateInfo, int> OnStateExitEvent;
    public event Action<Animator, AnimatorStateInfo, int> OnStateEnterEvent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (OnStateEnterEvent != null) {
            OnStateEnterEvent(animator, stateInfo, layerIndex);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (OnStateExitEvent != null) {
            OnStateExitEvent(animator, stateInfo, layerIndex);
        }
    }
    
}
