using UnityEngine;

namespace Tools {
    public abstract class AnimationStateChangesListener {
        
        public virtual void OnStateEntered(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        }

        public virtual void OnStateExited(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        }
        
    }
}