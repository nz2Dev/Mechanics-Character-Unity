using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class Door : MonoBehaviour {

    public GameObject Handle;
    public float CatchRadius = 1;
    public float opennedPositionAngle = -90;
    public float closedPositionAngle = 0;

    private HingeJoint doorHingeJoint;
    private BoxCollider doorCollider;

    private void Start() {
        doorHingeJoint = GetComponent<HingeJoint>();
        doorCollider = GetComponent<BoxCollider>();
    }

    public void Open() {
        doorHingeJoint.useSpring = false;
        doorCollider.enabled = true;

        var limits = doorHingeJoint.limits;
        limits.min = -90;
        limits.max = 90;
        doorHingeJoint.limits = limits;
    }

    public void KeepOpened() {
        doorHingeJoint.useSpring = false;
        //var spring = doorHingeJoint.spring;
        //spring.targetPosition = opennedPositionAngle;
        //spring.spring = 10;
        //doorHingeJoint.spring = spring;
        doorCollider.enabled = false;


        var limits = doorHingeJoint.limits;
        limits.min = -90;
        limits.max = 90;
        doorHingeJoint.limits = limits;
    }

    [ContextMenu("CloseTheDoor")]
    public void Close() {
        doorHingeJoint.useSpring = false;
        var limits = doorHingeJoint.limits;
        limits.min = 0;
        limits.max = 0;
        doorHingeJoint.limits = limits;
        doorCollider.enabled = true;
    }

}
