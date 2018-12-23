using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    private Character character;

    private Plane playersPlane = new Plane(Vector3.up, Vector3.zero);
    private Vector3 intersectPoint;

    private void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        character = player.GetComponent<Character>();
    }

    public void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        character.Move(new Vector3(horizontal, 0, vertical));

        if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enterDistance;

            playersPlane.SetNormalAndPosition(Vector3.up, character.transform.position);
            playersPlane.Raycast(ray, out enterDistance);
            intersectPoint = ray.GetPoint(enterDistance);

            character.LookAt(intersectPoint);
        }

        // Right DOWN
        if (Input.GetMouseButtonDown(1)) {
            // Together with looking at the point when right mouse click
            // We would also prepare our weapon
            character.PrepareAttack(true);
        }

        // Right UP
        if (Input.GetMouseButtonUp(1)) {
            // Seems that attack and move states should be handled at once 
            character.PrepareAttack(false);
        }

        // Left DOWN
        if (Input.GetMouseButtonDown(0)) {
            character.AimAttack(true);
        }

        // Left UP
        if (Input.GetMouseButtonUp(0)) {
            character.AimAttack(false);
        }
    }

}
