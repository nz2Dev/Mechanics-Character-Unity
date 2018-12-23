using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingAttackState : AttackState {

    public override void Prepare(AttackStateMediator attackMediator) {
        throw new Exception("Prepare should not be called in AimState");
    }

    public override void Unprepare(AttackStateMediator attackMediator) {
        // If we are not prepared in aim state, lets move into ambiguous preparing state,
        // So that after we will be prepared again, we should again enter aiming state
        attackMediator.SetState(attackMediator.ambiguousPreparingAttackState);
    }

    public override void Aim(AttackStateMediator attackMediator) {
        Debug.LogWarning("Aim called in Aim state. This calls was ignored.");
    }

    public override void Unaim(AttackStateMediator attackMediator) {
        // Because in case if we unprepared, we entering preparing state again,
        // So in thsi case, we can be shure that we preapred in can entering releasing state.
        attackMediator.SetState(attackMediator.releasingState);
    }

}
