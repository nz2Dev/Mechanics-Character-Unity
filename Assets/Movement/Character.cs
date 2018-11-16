using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private const float DampTime = 0.1f;

    [SerializeField] private float stationarTurnSpeed = 180;
    [SerializeField] private float movingTurnSpeed = 360;

    private Animator animator;
    private float turnAmount;
    private float forwardAmount;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void Move(Vector3 direction) {
        if (direction.magnitude > 1) {
            direction.Normalize();
        }

        direction = transform.InverseTransformDirection(direction);
        turnAmount = Mathf.Atan2(direction.x, direction.z);
        forwardAmount = direction.z;

        ApplyExtraTurnRotation();
        ApplyAnimation(direction);
    }

    private void ApplyAnimation(Vector3 direction) {
        animator.SetFloat("forward", direction.z, DampTime, Time.deltaTime);
        animator.SetFloat("turn", turnAmount, DampTime, Time.deltaTime);
    }

    private void ApplyExtraTurnRotation() {
        float turnSpeed = Mathf.Lerp(stationarTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(Vector3.up, turnAmount * Time.deltaTime * turnSpeed);
    }

}
