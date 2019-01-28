using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterBowDirector : MonoBehaviour {

    public Bow bow;
    public Arrow arrowPrefab;
    public Transform arrowOrientation;

    private Character character;
    private Animator animator;

    private Arrow arrowInUse;
    private bool disablingSupperFocusModeImmunitet;
    private bool changeSupperFocuseOnLaunched;
    private bool supperFoucseChangeBuffer;

    private void Start() {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
    }

    public void Aim(bool aim) {
        if (!aim && animator.GetBool("prepared")) {
            disablingSupperFocusModeImmunitet = true;
        }

        // Set upper layer animation related to bow
        animator.SetBool("aimed", aim);
        bool aimState = aim && animator.GetBool("prepared");
        SetSupperFocus(aimState);

        // Set base layer animation related to character movement
        bool focusState = aim || animator.GetBool("prepared");
        character.SetFocusMoveState(focusState);
    }

    public void Prepare(bool prepare) {
        // Set upper layer animation related to bow
        animator.SetBool("prepared", prepare);
        bool aimState = animator.GetBool("aimed") && prepare;
        SetSupperFocus(aimState);

        // Set base layer animation related to character movement
        bool focusState = prepare || animator.GetBool("aimed");
        character.SetFocusMoveState(focusState);
    }

    private void SetSupperFocus(bool set) {
        if (!set && disablingSupperFocusModeImmunitet) {
            changeSupperFocuseOnLaunched = true;
            supperFoucseChangeBuffer = set;
            return;
        }
        changeSupperFocuseOnLaunched = false;
        character.SetSuperFocusMoveState(set);
    }

    public void OnCatchArrow() {
        RightHand rightHand = character.GetComponentInChildren<RightHand>();
        arrowInUse = Instantiate(arrowPrefab, rightHand.transform);
        arrowInUse.transform.localPosition = arrowOrientation.position;
        arrowInUse.transform.localRotation = arrowOrientation.rotation;
    }

    public void OnCatchBowstring() {
        RightHand rightHand = character.GetComponentInChildren<RightHand>();
        bow.StickBowstringTo(rightHand.transform);
    }

    public void OnReleaseBowstringAndArrow() {
        bow.Release();
        arrowInUse.Launch(character.transform.forward);
        arrowInUse.transform.parent = null;
        StartCoroutine(PostLaunch());
    }

    public void OnReleaseBowstring() {
        bow.UnstickBowstring();
    }

    public void OnReleaseArrow() {
        Destroy(arrowInUse.gameObject);
    }

    private IEnumerator PostLaunch() {
        yield return new WaitForSeconds(0.5f);
        arrowInUse.Launched();
        arrowInUse = null;

        if (disablingSupperFocusModeImmunitet) {
            disablingSupperFocusModeImmunitet = false;

            if (changeSupperFocuseOnLaunched) {
                changeSupperFocuseOnLaunched = false;
                character.SetSuperFocusMoveState(supperFoucseChangeBuffer);
            }
        }
    }

}
