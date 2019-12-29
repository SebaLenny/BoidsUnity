using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoidController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class NewBehaviourScript : MonoBehaviour
{
    private BoidController bc;

    void Start()
    {
        bc = GetComponent<BoidController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        bc.ReportCollision(other.impulse.sqrMagnitude);
    }
}
