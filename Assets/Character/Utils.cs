using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

    public static bool IsAnimatorPlaying(Animator animator, string name) {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(name) && !animator.IsInTransition(0);
    }

}
