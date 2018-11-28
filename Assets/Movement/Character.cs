using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private const float DampTime = 0.1f;

    [SerializeField] private float stationarTurnSpeed = 180;
    [SerializeField] private float movingTurnSpeed = 360;
    [SerializeField] private Transform platformTransform;
    [SerializeField] private Transform headTransform;

    [Range(1, 180)]
    [SerializeField] private float headRotationAngle = 100;

    private Animator animator;
    private Vector3 localMoveDirection;
    private float turnAmount;
    private float forwardAmount;
    private Vector3 worldHeadForward;

    private bool headControlState;
    private Vector3 lookPoint;

    [SerializeField]
    private float headFollowRotationSpeed = 1;

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

            ApplyExtraTurnRotation();
        } else {
            turnAmount = localMoveDirection.x;
            forwardAmount = localMoveDirection.z;
        }

        ApplyAnimation();
    }

    public void LookAt(Vector3 point) {
        headControlState = true;
        worldHeadForward = (point - transform.position).normalized;
        lookPoint = point;
        animator.SetBool("faceFocuse", headControlState);
    }

    public void LookForward() {
        headTransform.localRotation = Quaternion.identity;
        headControlState = false;
        animator.SetBool("faceFocuse", headControlState);
    }

    private void Update() {
        if (headControlState) {
            float directionHeadAngle = Vector3.Angle(worldHeadForward, transform.forward);

            float rotationInfluence = Time.deltaTime * Mathf.Pow(directionHeadAngle / headRotationAngle, 2);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(worldHeadForward, Vector3.up), rotationInfluence);
            
            // if (turnTrigger != null) {
            //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //    if (stateInfo.IsName("Focuse Movement") && !animator.IsInTransition(0)) {
            //        animator.SetTrigger(turnTrigger);
            //    }
            // }
        }
    }

    private void LateUpdate() {
        if (headControlState) {
            // look in front of the point
            headTransform.LookAt(lookPoint);
            // headTransform.Rotate(new Vector3(0, 90, -90), Space.Self);
            Debug.DrawLine(headTransform.position, lookPoint, Color.black);

            // get direction of head in world space
            // for Ethan -Vector3.up direction is head local forward after rotation
            // worldHeadForward = Vector3.Scale(headTransform.TransformDirection(Vector3.forward), new Vector3(1, 0, 1)).normalized;
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
        }

        // Move indicator
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, localMoveDirection);
    }

    private void ApplyAnimation() {
        animator.SetFloat("forward", forwardAmount, DampTime, Time.deltaTime);
        animator.SetFloat("turn", turnAmount, DampTime, Time.deltaTime);
    }

    private void ApplyExtraTurnRotation() {
        float turnSpeed = Mathf.Lerp(stationarTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(Vector3.up, turnAmount * Time.deltaTime * turnSpeed);
    }

}
