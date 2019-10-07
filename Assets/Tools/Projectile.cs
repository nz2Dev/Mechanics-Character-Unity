using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour {

    private Rigidbody physics;
    private Collider bounds;
    private bool enableDetecting;
    
    public Collider[] collisionFreeObjects;
    public LayerMask collisionLayer;

    private Quaternion launchRotation;

    private void Awake()
    {
        physics = GetComponent<Rigidbody>();
        physics.constraints = RigidbodyConstraints.FreezeAll;
        bounds = GetComponent<Collider>();
        bounds.enabled = false;
    }

    void Start() {
        
    }

    public void Launch() {
        foreach (var collisionFreeObject in collisionFreeObjects)
        {
            if (collisionFreeObject)
            {
                Physics.IgnoreCollision(bounds, collisionFreeObject);
            }
        }
        
        physics.constraints = RigidbodyConstraints.FreezeRotation;
        physics.AddForce(transform.forward * 10, ForceMode.VelocityChange);
        
        transform.parent = null;
        bounds.enabled = true;
        
//        launchRotation = transform.rotation;
    }

    public void LateUpdate()
    {
//        if (launchRotation != null) {
//            transform.rotation = launchRotation;
//        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!collisionLayer.Contains(collision.gameObject.layer))
        {
            return;
        }
        Destroy(gameObject);
    }

}
