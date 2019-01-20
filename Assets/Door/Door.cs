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
    }

    public void KeepOpened() {
        doorHingeJoint.useSpring = true;
        var spring = doorHingeJoint.spring;
        spring.targetPosition = opennedPositionAngle;
        spring.spring = 10;
        doorHingeJoint.spring = spring;
        doorCollider.enabled = false;
    }

    [ContextMenu("CloseTheDoor")]
    public void Close() {
        doorHingeJoint.useSpring = true;
        var spring = doorHingeJoint.spring;
        spring.spring = 10000;
        spring.targetPosition = closedPositionAngle;
        doorHingeJoint.spring = spring;
        doorCollider.enabled = true;
    }

}
