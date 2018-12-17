using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour {

    private Rigidbody physics;
    private Collider bounds;
    private bool enableDetecting;

    void Start() {
        physics = GetComponent<Rigidbody>();
        physics.constraints = RigidbodyConstraints.FreezeAll;
        bounds = GetComponent<Collider>();
        bounds.enabled = false;
    }

    public void Launch(Vector3 direction) {
        physics.constraints = RigidbodyConstraints.FreezeRotation;
        physics.AddForce(transform.forward * 10, ForceMode.VelocityChange);
    }

    public void Launched() {
        bounds.enabled = true;
    }

    private void OnCollisionEnter(Collision collision) {
        // TODO callback
        Destroy(gameObject);
    }

}
