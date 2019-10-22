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
    [Range(0f, 180f)]
    public float angleThreshold = 90f;
    [Range(0f, 5f)]
    public float aligmentStrenght = .1f;
    [Range(0f, 5f)]
    public float separationStrenght = .5f;
    [Range(0f, 5f)]
    public float cohesionStrenght = 1f;
    [Range(0f, 5f)]
    public float reaccelerationForce = 1f;
    public bool observe = false;
    private void Start()
    {
        velocity = startVelocity + new Vector3(Random.Range(-randomStartingVelocity, randomStartingVelocity), Random.Range(-randomStartingVelocity, randomStartingVelocity), Random.Range(-randomStartingVelocity, randomStartingVelocity));
    }

    private void FixedUpdate()
    {
        AdvanceSimulation();
    }

    public void AdvanceSimulation()
    {
        List<BoidController> boids = getNerbyBoids();

        ApplySeparation(boids);
        ApplyAligment(boids);
        ApplyCohesion(boids);

        ApplyVectors();

        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        acceleration = velocity.normalized * reaccelerationForce;
    }

    private void ApplyVectors()
    {
        acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);
        velocity += acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private List<BoidController> getNerbyBoids()
    {
        List<BoidController> boids = Physics.OverlapSphere(transform.position, maxRadious, 1 << LayerMask.NameToLayer("Boids"))
        .Select(c => c.GetComponent<BoidController>())
        .Where(c => c != null).ToList(); // use non aloc and try to settle for masks for boids separately
        boids.Remove(this);
        return boids;
    }

    public void ApplySeparation(List<BoidController> boids)
    {
        if (boids.Count == 0) return;
        Vector3 averageForce = Vector3.zero;
        foreach (var boid in boids)
        {
            if (Vector3.Angle(this.velocity, boid.velocity) < angleThreshold)
            {
                Vector3 direction = (transform.position - boid.transform.position).normalized;
                float distance = (transform.position - boid.transform.position).magnitude;
                float bias = Mathf.Lerp(maxRadious, 0, distance * distance);
                averageForce += direction * bias;
            }
        }
        averageForce /= boids.Count;
        this.acceleration += averageForce * separationStrenght;// * 10;
    }

    public void ApplyAligment(List<BoidController> boids)
    {
        if (boids.Count == 0) return;
        Vector3 average = Vector3.zero;
        foreach (var boid in boids)
        {
            if (Vector3.Angle(this.velocity, boid.velocity) < angleThreshold)
            {
                average += boid.Velocity;
            }
        }
        average /= boids.Count;
        this.acceleration += (average - this.velocity) * aligmentStrenght;
    }

    public void ApplyCohesion(List<BoidController> boids)
    {
        if (boids.Count == 0) return;
        Vector3 averagePosition = Vector3.zero;
        foreach (var boid in boids)
        {
            if (Vector3.Angle(this.velocity, boid.velocity) < angleThreshold)
            {
                averagePosition += boid.transform.position;
            }
        }
        averagePosition /= boids.Count;
        this.acceleration += (averagePosition - transform.position) * cohesionStrenght;
    }

    public void DrawDebugs()
    {
        Debug.DrawLine(transform.position, transform.position + velocity, Color.red, 0f);
        Debug.DrawLine(transform.position, transform.position + acceleration, Color.green, 0f);
    }

    private void OnDrawGizmos()
    {
        if (observe)
        {
            DrawDebugs();
            Gizmos.DrawWireSphere(transform.position, maxRadious);
        }
    }
}
