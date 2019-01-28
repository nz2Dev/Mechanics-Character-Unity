﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Character))]
public class CharacterSwordDirector : MonoBehaviour {

    private Animator animator;
    private Character character;

    private void Start() {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
    }

    public void Prepare(bool prepare) {
        // Set upper layer animation related to bow
        animator.SetBool("prepared", prepare);
        bool aimState = animator.GetBool("aimed") && prepare;
        character.SetSuperFocusMoveState(aimState);

        // Set base layer animation related to character movement
        bool focusState = prepare || animator.GetBool("aimed");
        character.SetFocusMoveState(focusState);
    }

    public void Aim(bool aim) {
        // Set upper layer animation related to bow
        animator.SetBool("aimed", aim);
        bool aimState = aim && animator.GetBool("prepared");
        character.SetSuperFocusMoveState(aimState);

        // Set base layer animation related to character movement
        bool focusState = aim || animator.GetBool("prepared");
        character.SetFocusMoveState(focusState);
    }

}
