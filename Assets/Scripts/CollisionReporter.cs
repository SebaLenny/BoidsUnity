using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScoreUpcaster))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CollisionReporter : MonoBehaviour
{
    private ScoreUpcaster su;

    void Awake()
    {
        su = GetComponent<ScoreUpcaster>();
    }

    private void OnCollisionEnter(Collision other)
    {
        su.ReportCollision(Mathf.Min(other.impulse.sqrMagnitude, 1));
    }
}
