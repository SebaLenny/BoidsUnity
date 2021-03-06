﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class BoidController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 accelerationToApply;
    public Vector3 startVelocity;
    public RuleSet ruleSet;
    public GameObject currentTarget;
    public bool observe = false;
    private static int obstacleLayer;
    private static int boidsLayer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        obstacleLayer = 1 << LayerMask.NameToLayer("Obstacles");
        boidsLayer = 1 << LayerMask.NameToLayer("Boids");
    }

    private void FixedUpdate()
    {
        accelerationToApply += GetForces();
        ApplyVectors();
        if(rb.velocity != Vector3.zero) transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        if (observe) DrawDebugs();
    }

    public Vector3 GetForces()
    {
        List<BoidController> boids = GetNerbyBoids();
        Vector3 force = Vector3.zero;
        if (ruleSet.separation.isActive)
            force += ApplySeparation(boids.Where(b => Vector3.Distance(transform.position, b.transform.position) < ruleSet.separation.range).ToList()) * ruleSet.separation.strength; // might be slow
        if (ruleSet.alignment.isActive)
            force += ApplyAlignment(boids.Where(b => Vector3.Distance(transform.position, b.transform.position) < ruleSet.alignment.range).ToList()) * ruleSet.alignment.strength; // might be slow
        if (ruleSet.cohesion.isActive)
            force += ApplyCohesion(boids.Where(b => Vector3.Distance(transform.position, b.transform.position) < ruleSet.cohesion.range).ToList()) * ruleSet.cohesion.strength; // might be slow
        if (ruleSet.collisionAvoidance.isActive)
            force += ApplyCollisiton() * ruleSet.collisionAvoidance.strength;
        if (ruleSet.targetChasing.isActive && currentTarget != null)
            force += ApplyChase() * ruleSet.targetChasing.strength;
        return force;
    }

    private void ApplyVectors()
    {
        accelerationToApply = Vector3.ClampMagnitude(accelerationToApply, ruleSet.maxAcceleration);
        rb.AddForce(accelerationToApply);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, ruleSet.maxVelocity);
        //transform.position += rb.velocity * Time.fixedDeltaTime;
    }

    private List<BoidController> GetNerbyBoids()
    {
        List<BoidController> boids = Physics.OverlapSphere(transform.position, getMaxRadious(), boidsLayer)
        .Select(c => c.GetComponent<BoidController>())
        .Where(c => c != null).ToList(); // use non aloc and try to settle for masks for boids separately
        boids.Remove(this);
        return boids;
    }

    private float getMaxRadious()
    {
        return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(ruleSet.targetChasing.range, ruleSet.collisionAvoidance.range), ruleSet.separation.range), ruleSet.cohesion.range), ruleSet.alignment.range);
    }

    public Vector3 ApplySeparation(List<BoidController> boids)
    {
        if (boids.Count == 0) return Vector3.zero;
        if (!ruleSet.separation.considerOtherGroups)
        {
            boids = boids.Where(b => b.ruleSet == ruleSet).ToList();
        }
        Vector3 averageForce = Vector3.zero;
        int count = 0;
        foreach (var boid in boids)
        {
            if (isSeeing(boid))
            {
                Vector3 diff = transform.position - boid.transform.position;
                float dist = 2 / (Mathf.Pow(diff.magnitude, 2) + 0.00001f);
                averageForce += diff * dist;
                count++;
            }
        }
        if (count != 0)
            averageForce /= count;
        return averageForce;
    }

    public Vector3 ApplyAlignment(List<BoidController> boids)
    {
        if (boids.Count == 0) return Vector3.zero;
        if (!ruleSet.alignment.considerOtherGroups)
        {
            boids = boids.Where(b => b.ruleSet == ruleSet).ToList();
        }
        Vector3 averageVelocity = Vector3.zero;
        int count = 0;
        foreach (var boid in boids)
        {
            if (isSeeing(boid))
            {
                averageVelocity += boid.rb.velocity;
                count++;
            }
        }
        if (count != 0)
            averageVelocity /= count;
        return averageVelocity;
    }

    public Vector3 ApplyCohesion(List<BoidController> boids)
    {
        if (boids.Count == 0) return Vector3.zero;
        if (!ruleSet.cohesion.considerOtherGroups)
        {
            boids = boids.Where(b => b.ruleSet == ruleSet).ToList();
        }
        Vector3 averagePosition = Vector3.zero;
        int count = 0;
        foreach (var boid in boids)
        {
            if (isSeeing(boid))
            {
                averagePosition += boid.transform.position;
                count++;
            }
        }
        if (count == 0) return Vector3.zero;
        return averagePosition - transform.position;
    }

    private Vector3 ApplyCollisiton()
    {
        Quaternion rotation =
        Quaternion.FromToRotation(transform.position, rb.velocity);
        Vector3 force = Vector3.zero;
        int hits = 0;
        foreach (var point in DirectionHelper.PointsOnSphere)
        {
            Vector3 rotatedPoint = (rotation * point) * ruleSet.collisionAvoidance.range;
            if (isSeeing(transform.position + rotatedPoint))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, rotatedPoint, out hit, ruleSet.collisionAvoidance.range, obstacleLayer))
                {
                    float magnitude = 2 / (hit.distance + 0.01f);
                    force -= rotatedPoint * magnitude;
                    hits += 1;
                    if (observe)
                    {
                        Debug.DrawLine(transform.position, hit.point, new Color(1, 1, 0, .2f));
                    }
                }
                else
                {
                    if (observe)
                    {
                        Debug.DrawRay(transform.position, rotatedPoint, new Color(1, 0, 1, .1f));
                    }
                }
            }
        }
        return hits != 0 ? force : Vector3.zero;
    }

    private Vector3 ApplyChase()
    {
        return (2 * (currentTarget.transform.position - transform.position).normalized) - rb.velocity.normalized;
    }

    public bool isSeeing(BoidController otherBoid)
    {
        return Vector3.Angle(rb.velocity, otherBoid.transform.position - transform.position) < ruleSet.seeAngle;
    }

    public bool isSeeing(Vector3 point)
    {
        return Vector3.Angle(rb.velocity, point - transform.position) < ruleSet.seeAngle;
    }

    public void DrawDebugs()
    {
        Debug.DrawLine(transform.position, transform.position + rb.velocity, new Color(1, 0, 0, .2f), 0f);
        Debug.DrawLine(transform.position, transform.position + accelerationToApply, Color.green, 0f);
    }
}
