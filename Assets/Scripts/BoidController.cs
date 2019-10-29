using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoidController : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 acceleration;
    public Vector3 startVelocity;
    [Range(0f, 1f)]
    public float randomStartingVelocity = 0f;
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    public Vector3 Acceleration { get { return acceleration; } set { velocity = value; } }
    public float maxVelocity = 5;
    public float maxAcceleration = 1;
    public float maxRadious;
    // [Range(0f, 180f)]
    // public float angleThreshold = 90f;
    // [Range(0f, 5f)]
    // public float aligmentStrenght = .1f;
    // [Range(0f, 5f)]
    // public float separationStrenght = .5f;
    // [Range(0f, 5f)]
    // public float cohesionStrenght = 1f;
    // [Range(0f, 5f)]
    // public float reaccelerationForce = 1f;
    public bool observe = false;
    private void Start()
    {
        velocity = startVelocity + new Vector3(Random.Range(-randomStartingVelocity, randomStartingVelocity), Random.Range(-randomStartingVelocity, randomStartingVelocity), Random.Range(-randomStartingVelocity, randomStartingVelocity));
    }

    private void FixedUpdate()
    {
        ApplyBounds();
        acceleration += GetForces();
        ApplyVectors();
        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        if (observe)
        {
            DrawDebugs();
        }
    }

    private void ApplyBounds()
    {
        Vector3 pos = transform.position;
        Vector3 newPos = pos;
        Vector3 bound = MasterController.Instance.bounds;
        if (pos.x > bound.x) newPos.x = -bound.x;
        if (pos.x < -bound.x) newPos.x = bound.x;
        if (pos.y > bound.y) newPos.y = -bound.y;
        if (pos.y < -bound.y) newPos.y = bound.y;
        if (pos.z > bound.z) newPos.z = -bound.z;
        if (pos.z < -bound.z) newPos.z = bound.z;
        transform.position = newPos;
    }

    public Vector3 GetForces()
    {
        List<BoidController> boids = GetNerbyBoids();
        Vector3 force = Vector3.zero;
        if (MasterController.Instance.separation)
            force += ApplySeparation(boids) * MasterController.Instance.separationStrenght;
        if (MasterController.Instance.aligment)
            force += ApplyAligment(boids) * MasterController.Instance.aligmentStrenght;
        if (MasterController.Instance.cohesion)
            force += ApplyCohesion(boids) * MasterController.Instance.cohesionStrenght;
        if (MasterController.Instance.collision)
            force += ApplyCollisiton() * MasterController.Instance.collisionStrenght;
        if (MasterController.Instance.chaseTarget && MasterController.Instance.target != null)
            force += ApplyChase() * MasterController.Instance.targetStrenght;
        return force;
    }

    private void ApplyVectors()
    {
        acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);
        velocity += acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private List<BoidController> GetNerbyBoids()
    {
        List<BoidController> boids = Physics.OverlapSphere(transform.position, maxRadious, 1 << LayerMask.NameToLayer("Boids"))
        .Select(c => c.GetComponent<BoidController>())
        .Where(c => c != null).ToList(); // use non aloc and try to settle for masks for boids separately
        boids.Remove(this);
        return boids;
    }

    public Vector3 ApplySeparation(List<BoidController> boids)
    {
        if (boids.Count == 0) return Vector3.zero;
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

    public Vector3 ApplyAligment(List<BoidController> boids)
    {
        if (boids.Count == 0) return Vector3.zero;
        Vector3 averageVelocity = Vector3.zero;
        int count = 0;
        foreach (var boid in boids)
        {
            if (isSeeing(boid))
            {
                averageVelocity += boid.velocity;
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
        if (count != 0)
            averagePosition /= count;
        return averagePosition - transform.position;
    }

    private Vector3 ApplyCollisiton()
    {
        Quaternion rotation =
        Quaternion.FromToRotation(transform.position, velocity);
        Vector3 force = Vector3.zero;
        int hits = 0;
        foreach (var point in MasterController.Instance.PointsOnSphere)
        {
            Vector3 rotatedPoint = (rotation * point) * maxRadious;
            if (isSeeing(transform.position + rotatedPoint))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, rotatedPoint, out hit, maxRadious, 1 << LayerMask.NameToLayer("Obstacles")))
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
        if (hits == 0)
        {
            return Vector3.zero;
        }
        else
        {
            return force;
        }

    }

    private Vector3 ApplyChase()
    {
        return (MasterController.Instance.target.transform.position - transform.position).normalized;
    }

    public bool isSeeing(BoidController otherBoid)
    {
        return Vector3.Angle(velocity, otherBoid.transform.position - transform.position) < MasterController.Instance.angleThreshold;
    }

    public bool isSeeing(Vector3 point)
    {
        return Vector3.Angle(velocity, point - transform.position) < MasterController.Instance.angleThreshold;
    }


    public void DrawDebugs()
    {
        Debug.DrawLine(transform.position, transform.position + velocity, new Color(1, 0, 0, .2f), 0f);
        Debug.DrawLine(transform.position, transform.position + acceleration, Color.green, 0f);
    }

    private void OnDrawGizmos()
    {
        if (observe)
        {
            //Gizmos.DrawWireSphere(transform.position, maxRadious);
        }
    }
}
