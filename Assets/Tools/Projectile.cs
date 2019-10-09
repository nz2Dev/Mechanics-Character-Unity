using System.Collections;
using Tools;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour {

    private Rigidbody physics;
    private Collider bounds;
    private bool enableDetecting;
    
    public Collider[] collisionFreeObjects;
    public LayerMask collisionLayerMask;

    private void Awake()
    {
        physics = GetComponent<Rigidbody>();
        physics.constraints = RigidbodyConstraints.FreezeAll;
        bounds = GetComponent<Collider>();
        bounds.enabled = false;
    }

    public void Launch() {
        foreach (var collisionFreeObject in collisionFreeObjects)
        {
            if (collisionFreeObject)
            {
                Physics.IgnoreCollision(bounds, collisionFreeObject);
            }
        }

        bounds.enabled = true;
        transform.parent = null;

        physics.constraints = RigidbodyConstraints.FreezeRotation;
        physics.AddForce(transform.forward * 10, ForceMode.VelocityChange);
    }

    public void Fall() {
        physics.useGravity = true;
        physics.constraints = RigidbodyConstraints.None;
        bounds.enabled = true;
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer() {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collisionLayerMask.Contains(collision.gameObject.layer))
        {
            Destroy(gameObject);
        }
    }

}
