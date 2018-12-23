using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public const int UpperLayerIndex = 2;
    public const string UpperLayerIdleStateInfoName = "Idle";

    private const float DampTime = 0.1f;

    [SerializeField] private float stationarTurnSpeed = 180;
    [SerializeField] private float movingTurnSpeed = 360;
    [SerializeField] private Transform platformTransform;
    [SerializeField] private Transform spineTransform;
    [SerializeField] private Transform headTransform;
    [SerializeField] [Range(1, 180)] private float headRotationAngle = 100;
    [SerializeField] private float headFollowRotationSpeed = 1;
    [SerializeField] private float undefinedMultiplyer = 2;

    [SerializeField] private WeaponDirector weaponDirector;

    private Animator animator;

    private Vector3 localMoveDirection;
    private Vector3 worldHeadForward;
    private Vector3 lookPoint;
    private float turnAmount;
    private float forwardAmount;
    private bool headControlState;
    private bool aimState;

    private AttackStateMediator attackMediator;

    private void Start() {
        animator = GetComponent<Animator>();

        // It is not a good idea to initialize mediator here, 
        // but rather taking it as dependencies in some way or have some mechanizm to set it dynamically to have posibility to extend it in future.
        // But for now, let it be like that, hardcoded.
        attackMediator = new AttackStateMediator {
            idleState = new IdleAttackState(),
            preparingState = new PreparingAttackState(),
            ambiguousPreparingAttackState = new AmbiguousPreparingAttackState(),
            aimingState = new AimingAttackState(),
            releasingState = new ReleasingAttackState(),
        };
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

    public void PrepareAttack(bool prepare) {
        // It will be moved to implementation of attackDirector events handling
        // And be called from there.
        // if (weaponDirector != null) {
        //     weaponDirector.OnPrepare();
        // }

        // The same for animations trigger. moreover, maybe this should be called from weapon director implementation.
        // animator.SetTrigger("reload");

        // attackMediator.PrepareAttack(prepare);
        animator.SetBool("prepared", prepare);

        aimState = animator.GetBool("aimed") || prepare;
    }

    public void AimAttack(bool aim) {
        // if (weaponDirector != null) {
        //     weaponDirector.OnAim();
        // }

        // animator.SetTrigger("aim");
        
        // Thre same here as in PrepareAttack()
        // attackMediator.AimAttack(aim);
        animator.SetBool("aimed", aim);

        aimState = aim || animator.GetBool("prepared");

        // TODO: character hips should look at point, maybe using lerp smooth, but should do it slower that face
    }

    // public void Attack() {
    // if (weaponDirector != null) {
    //     weaponDirector.OnShot();
    // }

    // animator.SetTrigger("attack");
    // aimState = false;

    // NOTE: should be handled by attackMediator after some of Prepare or Aim calls.

    // TODO: freez head and hips \and platform/ during attack animation?
    // }

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
