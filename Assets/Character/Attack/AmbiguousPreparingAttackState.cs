using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiguousPreparingAttackState : AttackState {

    public override void Aim(AttackStateMediator attackMediator) {
        // In this state aiming means that we in preparing state. So we can simply ignore this.
        Debug.LogWarning("Aim called in AmbiguousPreparingAttackState. This cals was ignored");
    }

    public override void Prepare(AttackStateMediator attackMediator) {
        // We reenter aiming state when we is prepared again.
        attackMediator.SetState(attackMediator.aimingState);
    }

    public override void Unaim(AttackStateMediator attackMediator) {
        // When we unaim in this state, we simply have no choices rather that going to idle state.
        attackMediator.SetState(attackMediator.idleState);
    }

    public override void Unprepare(AttackStateMediator attackMediator) {
        // Unpreparing in this state should have no effect, So we can simpy ignore thhis.
        // TODO: Deside what politics to use in this cases. Throw an exceptions, or ignore this with warnings, or distinguish this and error cases as different.
        // TODO: Add specific class of state exceptions that will simplify errors displaying and formating.
        Debug.LogWarning("Unprepare called in AmbiguousPreparingAttackState. This cals was ignored");
    }

}
