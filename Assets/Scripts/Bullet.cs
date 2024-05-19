using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision other)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
