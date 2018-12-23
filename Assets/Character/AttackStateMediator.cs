using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMediator {

    public delegate void StateChangesEvent(AttackStateMediator attackMediator, AttackState previousState, AttackState nextState);

    public event StateChangesEvent OnStateChanged;

    public AttackState idleState;
    public AttackState preparingState;
    public AttackState ambiguousPreparingAttackState;
    public AttackState aimingState;
    public AttackState releasingState;

    private AttackState currentState;

    public AttackStateMediator() {}

    public void PrepareAttack(bool prepare) {
        if (prepare) {
            currentState.Prepare(this);
        } else {
            currentState.Unprepare(this);
        }
    }

    public void AimAttack(bool aim) {
        if (aim) {
            currentState.Aim(this);
        } else {
            currentState.Unaim(this);
        }
    }

    internal void SetState(AttackState attackState) {
        AttackState previousState = currentState;
        currentState = attackState;
        currentState.OnStateEntered(this, previousState);
        if (OnStateChanged != null) {
            OnStateChanged(this, previousState, attackState);
        }
    }

}
