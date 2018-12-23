using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAttackState : AttackState {

    public override void Prepare(AttackStateMediator attackMediator) {
        attackMediator.SetState(attackMediator.preparingState);
    }

    public override void Unprepare(AttackStateMediator attackMediator) {
        throw new Exception("Can't unprepare in Idle State");
    }

    public override void Aim(AttackStateMediator attackMediator) {
        throw new Exception("Can't aim in Idle State");
    }

    public override void Unaim(AttackStateMediator attackMediator) {
        throw new Exception("Can't unaim in Idle State");
    }
    
}
