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
        if (!focusState) {
            character.LookStraight();
        }
    }

    public void Prepare(bool prepare) {
        // Set upper layer animation related to bow
        animator.SetBool("prepared", prepare);
        bool aimState = animator.GetBool("aimed") && prepare;
        SetSupperFocus(aimState);

        // Set base layer animation related to character movement
        bool focusState = prepare || animator.GetBool("aimed");
        if (!focusState) {
            character.LookStraight();
        }
    }

    private void SetSupperFocus(bool set) {
        if (!set && disablingSupperFocusModeImmunitet) {
            changeSupperFocuseOnLaunched = true;
            supperFoucseChangeBuffer = set;
            return;
        }
        changeSupperFocuseOnLaunched = false;
        character.SetAimedSpineMode(set);
    }

    public void OnCatchArrow() {
        RightHand rightHand = character.GetComponentInChildren<RightHand>();
        arrowInUse = Instantiate(arrowPrefab, rightHand.transform);
        arrowInUse.transform.localPosition = arrowOrientation.position;
        arrowInUse.transform.localRotation = arrowOrientation.rotation;
    }

    public void OnCatchBowstring() {
        RightHand rightHand = character.GetComponentInChildren<RightHand>();
        arrowInUse.collisionFreeObjects = new[] {character.GetComponentInChildren<LeftHand>().GetComponent<Collider>()};
        bow.StickBowstringTo(rightHand.transform);
        bow.LoadArrow(arrowInUse);
    }

    public void OnReleaseBowstringAndArrow() {
        bow.Release();
        StartCoroutine(PostLaunch());
    }

    public void OnReleaseBowstring() {
        bow.UnstickBowstring();
        // probably it would require to unload arrow, and set it back to right hand
    }

    public void OnReleaseArrow() {
        // it should be called from animation when arrow is loaded back into quiver
        // so probable its better to rename it appropriately
        if (arrowInUse)
        {
            Destroy(arrowInUse.gameObject);
        }
    }

    private IEnumerator PostLaunch() {
        yield return new WaitForSeconds(0.5f);
        
        if (disablingSupperFocusModeImmunitet) {
            disablingSupperFocusModeImmunitet = false;

            if (changeSupperFocuseOnLaunched) {
                changeSupperFocuseOnLaunched = false;
                character.SetAimedSpineMode(supperFoucseChangeBuffer);
            }
        }
    }

}
