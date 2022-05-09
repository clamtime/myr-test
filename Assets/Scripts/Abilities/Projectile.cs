using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void SetForceDir(Vector3 dir, float force)
    {
        rb.AddForce(dir.normalized * force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") && transform.childCount > 0)
        {
            BroadcastMessage("ApplyAffector", collision.gameObject, SendMessageOptions.DontRequireReceiver);
        }
        Destroy(gameObject);        
    }
}
