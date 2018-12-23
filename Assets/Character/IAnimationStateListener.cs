using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationStateListener {

    void OnStateEntered(AnimatorStateInfo stateInfo, int characterAnimatorLayer);

}
