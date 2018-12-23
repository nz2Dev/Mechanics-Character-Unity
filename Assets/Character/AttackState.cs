using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState {

    public abstract void Prepare(AttackStateMediator attackMediator);
    public abstract void Unprepare(AttackStateMediator attackMediator);
    public abstract void Aim(AttackStateMediator attackMediator);
    public abstract void Unaim(AttackStateMediator attackMediator);

    public virtual void OnStateEntered(AttackStateMediator attackMediator, AttackState previousState) {
        // TODO: Consider save previous state as protected field, and update it there.
        // And also have this method as hook, so that any subclasses can hook into this.
    }

}
