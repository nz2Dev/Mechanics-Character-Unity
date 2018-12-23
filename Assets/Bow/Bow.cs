using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    private const float ReleasedTreashold = 0.1f;

    public Manipulator manipulator;
    public float bowstringStrength = 1;

    private Transform handlerTransform;
    private Vector3 manipulatorStartPosition;
    private Vector3 returnDirection;
    private bool releasing;
    private bool sticking;

    public void StickBowstringTo(Transform handler) {
        handlerTransform = handler;
        manipulatorStartPosition = manipulator.transform.localPosition;
        releasing = false;
        sticking = true;
    }

    public void UnstickBowstring() {
        // TODO consider using different approach, because just releasing might be buggy
        Release();
    }

    public void Release() {
        releasing = true;
        sticking = false;
        returnDirection = manipulatorStartPosition - manipulator.transform.localPosition;
    }

    private void Update() {
        if (sticking) {
            manipulator.transform.position = handlerTransform.position;
        }
        if (releasing) {
            manipulator.transform.localPosition = CalculateNextReleasePosition();
            if (Vector3.Distance(manipulator.transform.localPosition, manipulatorStartPosition) < ReleasedTreashold) {
                releasing = false;
                manipulator.transform.localPosition = manipulatorStartPosition;
            }
        }
    }

    private Vector3 CalculateNextReleasePosition() {
        return manipulator.transform.localPosition + returnDirection * Time.deltaTime * bowstringStrength;
    }

}
