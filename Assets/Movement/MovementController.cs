using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    private Character character;

    private Plane playersPlane = new Plane(Vector3.up, Vector3.zero);
    private Vector3 intersectPoint;

    private void Start() {
        character = GetComponent<Character>();
    }

    public void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        character.Move(new Vector3(horizontal, 0, vertical));

        if (Input.GetMouseButton(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enterDistance;

            playersPlane.SetNormalAndPosition(Vector3.up, character.transform.position);
            playersPlane.Raycast(ray, out enterDistance);
            intersectPoint = ray.GetPoint(enterDistance);

            character.LookAt(intersectPoint);
        }

        if (Input.GetMouseButtonUp(1)) {
            character.LookForward();
        }

        if (Input.GetMouseButtonDown(0)) {
            character.Aim();
        }

        if (Input.GetMouseButtonUp(0)) {
            character.Attack();
        }
    }

}
