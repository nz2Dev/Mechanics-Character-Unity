using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEditorInternal;
using UnityEngine;
using AnimatorController = UnityEditor.Animations.AnimatorController;

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
    [SerializeField] private int rotationInfluenceLimit = 1;
    
    private Animator animator;

    private Vector3 localMoveDirection;
    private Vector3 worldHeadForward;
    private Vector3? lookPoint;
    private float turnAmount;
    private float forwardAmount;
    private bool aimedSpineMode;

    private AnimationStatesChangesController stateChangesController;

    private void Awake() {
        animator = GetComponent<Animator>();
        stateChangesController = new AnimationStatesChangesController(animator);
    }

    public AnimationStatesChangesController GetAnimationStatesChangesController() {
        return stateChangesController;
    }
    
    public void Move(Vector3 direction) {
        if (direction.magnitude > 1) {
            direction.Normalize();
        }

        localMoveDirection = transform.InverseTransformDirection(direction);
        if (!lookPoint.HasValue) {
            var localX = localMoveDirection.x;
            var localY = localMoveDirection.z;

            turnAmount = Mathf.Atan2(localX, localY);
            forwardAmount = localMoveDirection.z;

            // Apply extra turn rotation when we in circular move mode
            var turnSpeed = Mathf.Lerp(stationarTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(Vector3.up, turnAmount * Time.deltaTime * turnSpeed);
        } else {
            turnAmount = localMoveDirection.x;
            forwardAmount = localMoveDirection.z;
        }

        // Apply animations
        animator.SetFloat("forward", forwardAmount, DampTime, Time.deltaTime);
        animator.SetFloat("turn", turnAmount, DampTime, Time.deltaTime);
    }

    public void LookAt(Vector3 point) {
        lookPoint = point;
        worldHeadForward = (point - transform.position).normalized;
        animator.SetBool("faceFocuse", true);
    }

    public void LookStraight() {
        lookPoint = null;
        headTransform.localRotation = Quaternion.identity;
        animator.SetBool("faceFocuse", false);
    }

    public void SetAimedSpineMode(bool modeEnabled) {
        aimedSpineMode = modeEnabled;
    }

    public void AimAttack(bool aim) {
        animator.SetBool("aimed", aim);
    }

    public bool IsAttackAimed() {
        return animator.GetBool("aimed");
    }

    public void PrepareAttack(bool prepare) {
        animator.SetBool("prepared", prepare);
    }

    public bool IsAttackPrepared() {
        return animator.GetBool("prepared");
    }

    private void LateUpdate() {
        var headRotation = Quaternion.LookRotation(worldHeadForward, Vector3.up);
        var directionHeadAngle = Vector3.Angle(worldHeadForward, transform.forward);

        if (lookPoint.HasValue) {
            var headTurnCoefficient = directionHeadAngle / headRotationAngle;
            var rotationInfluence = Time.deltaTime * Mathf.Pow(Math.Min(headTurnCoefficient, rotationInfluenceLimit), 2);

            // Rotate character toward the head with head turn coefficient, 
            // so that if head is more away from body forward the faster we rotate
            transform.rotation = Quaternion.Lerp(transform.rotation, headRotation, rotationInfluence);
        }

        if (aimedSpineMode) {
            // Adding head rotation to spine rotation so that now hi is facing forward when aiming
            // todo ideally should also have some interpolation
            headRotation *= spineTransform.localRotation;
            
            // hotfix. Adding extra rotation to compensate hands direction in aim animation
            headRotation *= Quaternion.Euler(0, 10, 0);
            
            spineTransform.rotation = headRotation;
        }

        if (lookPoint.HasValue) {
            // Because of head is a last child go
            // It should go after spine transform rotation in order to have effect
            headTransform.LookAt(lookPoint.Value);
        }
    }

    private void OnDrawGizmos() {
        float lineDistance = 10;

        Gizmos.matrix = transform.localToWorldMatrix;
        if (lookPoint.HasValue) {
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
            Gizmos.DrawLine(headTransform.position, lookPoint.Value);
        }

        // Move indicator
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, localMoveDirection);
    }

}
