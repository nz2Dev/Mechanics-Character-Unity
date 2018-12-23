using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStatesWeapongAdapter {

    private WeaponDirector weaponDirector;

    public AttackStatesWeapongAdapter(WeaponDirector weaponDirector) {
        this.weaponDirector = weaponDirector;
    }

    public void OnAttackStatesChanged(AttackStateMediator mediator, AttackState previousState, AttackState nextState) {
        // From idle to preparing
        if (previousState == mediator.idleState && nextState == mediator.preparingState) {
            weaponDirector.OnPrepare();
            return;
        }

        // From preparing to aiming
        if (previousState == mediator.preparingState && nextState == mediator.aimingState) {
            weaponDirector.OnAim();
            return;
        }

        // From preparing to the begining. e.g hide the weapon.
        if (previousState == mediator.preparingState && nextState == mediator.idleState) {
            weaponDirector.OnHide();
        }

        // From aiming to releasing
        if (previousState == mediator.aimingState && nextState == mediator.releasingState) {
            weaponDirector.OnShot();
        }

        // From aiming to ambiguous, e.g revers aiming.
        if (previousState == mediator.aimingState && nextState == mediator.ambiguousPreparingAttackState) {
            weaponDirector.OnReverseAim();
        }

        // From ambiguous to aiming, just aim as usual.
        if (previousState == mediator.ambiguousPreparingAttackState && nextState == mediator.aimingState) {
            weaponDirector.OnAim();
        }

        // From ambiguous to 
    }

}
