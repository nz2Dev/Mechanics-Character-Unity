using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparingAttackState : AttackState {

    public override void Prepare(AttackStateMediator attackMediator) {
        Debug.LogWarning("Preparing called in PreparedState. This calls was ignored.");
    }

    public override void Unprepare(AttackStateMediator attackMediator) {
        attackMediator.SetState(attackMediator.idleState);
    }

    public override void Aim(AttackStateMediator attackMediator) {
        attackMediator.SetState(attackMediator.aimingState);
    }

    public override void Unaim(AttackStateMediator attackMediator) {
        // Unaiming in Prepare state should have no effect. But doing so seems that client
        // Implementing calls wrong, so in such a case is better to show some errors rather that just warnign message.
        // And in case of doubling the same calls like in case with calling "Prepare" tow times.
        throw new Exception("Unaiming in preparing state is unexpected");
    }

}
