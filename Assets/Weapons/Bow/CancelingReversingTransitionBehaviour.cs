using UnityEngine;

public class CancelingReversingTransitionBehaviour : StateMachineBehaviour {

    public float MinStartTime = 0.2f;
    
    private bool flag;
    
    public void Trigger() {
        flag = true;
    }

    public void ResetFlag() {
        flag = false;
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (flag && stateInfo.normalizedTime > MinStartTime) {
            flag = false;
            animator.PlayInFixedTime("Preparing", layerIndex, (1 - stateInfo.normalizedTime) * stateInfo.length);
        }
    }

}