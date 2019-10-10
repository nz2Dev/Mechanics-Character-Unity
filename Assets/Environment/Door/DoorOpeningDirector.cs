using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class DoorOpeningDirector : MonoBehaviour {

    [SerializeField]
    private float directionSolverMultiplier = 10;
    [SerializeField]
    private BoxCollider leftHandCollider;

    private Character character;
    private Animator animator;

    private Door door;
    private bool doorTouched;

    private void Start() {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        leftHandCollider.enabled = false;
    }

    private void LateUpdate() {
        if (door && doorTouched) {
            var direction = door.transform.position - character.transform.position;
            var normalDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
            normalDirection.Normalize();
            var lookRotation = Quaternion.LookRotation(normalDirection, Vector3.up);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, lookRotation, Time.deltaTime * directionSolverMultiplier);
        }
    }

    public void OpenDoor(Door door) {
        // it should not trigger this via animator, but rather from character component,
        // and from there it should call public method that will set the trigger
        // and override runtime animator so that it will place door opener in place of special movement animation clip
        animator.SetTrigger("special");
        this.door = door;
    }

    public void OnTouchTheDoor() {
        leftHandCollider.enabled = true;
        doorTouched = true;
        door.Open();
    }

    public void OnDetachTheDoor() {
        doorTouched = false;
        door.KeepOpened();
    }

    public void OnAnimatorIK(int layerIndex) {
        if (animator && door && doorTouched) {
            // TODO: figure out why force from the hand in this case is much less, and dumped at the end
            // Maybe use some extra force at the end, or interpolate weight related to the time of event
            
            //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            //animator.SetIKPosition(AvatarIKGoal.LeftHand, door.Handle.transform.position);
        }
    }

}
