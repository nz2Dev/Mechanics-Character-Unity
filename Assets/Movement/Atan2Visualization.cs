using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Atan2Visualization : MonoBehaviour {

    public Vector3 vector = Vector3.zero;

    private float normalDegree;
    private float vectorSwapDegree;

    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float enterDistance;

        plane.Raycast(ray, out enterDistance);
        Vector3 intersectPoint = ray.GetPoint(enterDistance);
        vector = (intersectPoint - transform.position);
        Vector3 vectorNormalized = vector.normalized;

        normalDegree = Mathf.Atan2(vectorNormalized.z, vectorNormalized.x);
        vectorSwapDegree = Mathf.Atan2(vectorNormalized.x, vectorNormalized.z);
	}

    private void OnDrawGizmos() {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, vector);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, VectorFromAngle(normalDegree));
        Handles.Label(VectorFromAngle(normalDegree), Mathf.Rad2Deg * normalDegree + "*");
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, VectorFromAngle(vectorSwapDegree));
        Handles.Label(VectorFromAngle(vectorSwapDegree), Mathf.Rad2Deg * vectorSwapDegree + "*");
    }

    private Vector3 VectorFromAngle(float angle) {
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
    }

}
