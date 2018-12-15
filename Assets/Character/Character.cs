using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private const float DampTime = 0.1f;

    [SerializeField] private float stationarTurnSpeed = 180;
    [SerializeField] private float movingTurnSpeed = 360;
    [SerializeField] private Transform platformTransform;
    [SerializeField] private Transform spineTransform;
    [SerializeField] private Transform headTransform;
    [SerializeField] [Range(1, 180)] private float headRotationAngle = 100;
    [SerializeField] private float headFollowRotationSpeed = 1;
    [SerializeField] private float undefinedMultiplyer = 2;

    private Animator animator;

    private Vector3 localMoveDirection;
    private Vector3 worldHeadForward;
    private Vector3 lookPoint;
    private float turnAmount;
    private float forwardAmount;
    private bool headControlState;
    private bool aimState;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void Move(Vector3 direction) {
        if (direction.magnitude > 1) {
            direction.Normalize();
        }

        localMoveDirection = transform.InverseTransformDirection(direction);
        if (!headControlState) {
            float localX = localMoveDirection.x;
            float localY = localMoveDirection.z;

            turnAmount = Mathf.Atan2(y: localX, x: localY);
            forwardAmount = localMoveDirection.z;

            // Aply extra turn rotation when we in circular move mode
            float turnSpeed = Mathf.Lerp(stationarTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(Vector3.up, turnAmount * Time.deltaTime * turnSpeed);
        } else {
            turnAmount = localMoveDirection.x;
            forwardAmount = localMoveDirection.z;
        }

        // Aply animations
        animator.SetFloat("forward", forwardAmount, DampTime, Time.deltaTime);
        animator.SetFloat("turn", turnAmount, DampTime, Time.deltaTime);
    }

    public void LookAt(Vector3 point) {
        headControlState = true;
        lookPoint = point;
        worldHeadForward = (point - transform.position).normalized;
        animator.SetBool("faceFocuse", headControlState);
    }

    public void LookForward() {
        headControlState = false;
        aimState = false;
        headTransform.localRotation = Quaternion.identity;
        animator.SetBool("faceFocuse", headControlState);
    }

    public void Aim() {
        animator.SetTrigger("aim");
        aimState = true;
        // TODO: character hips should look at point, maybe using lerp smooth, but should do it slower that face
    }

    public void Attack() {
        animator.SetTrigger("attack");
        aimState = false;
        // TODO: freez head and hips \and platform/ during attack animation?
    }

    private void LateUpdate() {
        Quaternion headRotation = Quaternion.LookRotation(worldHeadForward, Vector3.up);
        float directionHeadAngle = Vector3.Angle(worldHeadForward, transform.forward);

        if (headControlState) {
            float headTurnCoeff = directionHeadAngle / headRotationAngle;
            float rotationInfluence = Time.deltaTime * Mathf.Pow(headTurnCoeff, 2);

            // Rotate character toward the head with head turn coeff, 
            // so that if head is more away from body forward the faster we rotationg
            transform.rotation = Quaternion.Lerp(transform.rotation, headRotation, rotationInfluence);
        }

        if (aimState) {
            // Adding head rotation to spine rotation so that now hi is facing forward when aiming
            headRotation = headRotation * spineTransform.localRotation;
            spineTransform.rotation = headRotation;
        }

        if (headControlState) {
            // But head looking toward the point immediately
            // It should go after spine transform rotation is set
            headTransform.LookAt(lookPoint);
        }
    }

    private void OnDrawGizmos() {
        float lineDistance = 10;

        Gizmos.matrix = transform.localToWorldMatrix;
        if (headControlState) {
            Gizmos.color = Color.black;
            // move cos/sin degress start
            float moveCosSinStart = +90;

            // get right rotation bounds
            float headRotationRightBoundAngle = moveCosSinStart - headRotationAngle;
            Vector3 headRotationBoundRight = new Vector3(Mathf.Cos(Mathf.Deg2Rad * headRotationRightBoundAngle), 0, Mathf.Sin(Mathf.Deg2Rad * headRotationRightBoundAngle));
            Gizmos.DrawLine(Vector3.zero, headRotationBoundRight * lineDistance);

            // get left rotation bounds
            float headRotationLeftBoundAngle = moveCosSinStart + headRotationAngle;
            Vector3 headRotationBoundLeft = new Vector3(Mathf.Cos(Mathf.Deg2Rad * headRotationLeftBoundAngle), 0, Mathf.Sin(Mathf.Deg2Rad * headRotationLeftBoundAngle));
            Gizmos.DrawLine(Vector3.zero, headRotationBoundLeft * lineDistance);

            // draw head world direction
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + worldHeadForward * lineDistance / 2);

            // line toward look point
            Gizmos.color = Color.black;
            Gizmos.DrawLine(headTransform.position, lookPoint);
        }

        // Move indicator
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, localMoveDirection);
    }

}
