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
    //public Vector3 startAcceleration;
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    public Vector3 Acceleration { get { return acceleration; } set { velocity = value; } }
    public float maxVelocity;
    public float maxRadious;
    [Range(0f, 180f)]
    public float angleThreshold = 90f;
    [Range(0f, 1f)]
    public float aligmentStrenght = .1f;
    [Range(0f, 1f)]
    public float separationStrenght = .5f;
    [Range(0f, 1f)]
    public float cohesionStrenght = 1f;
    [Range(0f, 5f)]
    public float reaccelerationForce = 1f;

    private void Start()
    {
        velocity = startVelocity + new Vector3(Random.Range(-randomStartingVelocity, randomStartingVelocity), Random.Range(-randomStartingVelocity, randomStartingVelocity), Random.Range(-randomStartingVelocity, randomStartingVelocity));
        //acceleration = startAcceleration;
    }
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        AdvanceSimulation();
    }

    public void AdvanceSimulation()
    {
        Collider[] referencedBoidsColliders = Physics.OverlapSphere(transform.position, maxRadious, 7); // use non aloc and try to settle for masks for boids separately
        List<BoidController> boids = referencedBoidsColliders.Select(c => c.GetComponent<BoidController>()).Where(c => c != null).ToList();
        boids.Remove(this);

        ApplySeparation(boids);
        ApplyAligment(boids);
        ApplyCohesion(boids);
        velocity += acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        transform.position += velocity * Time.fixedDeltaTime;
        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        acceleration = velocity.normalized * reaccelerationForce;
    }

    public void DrawDebugs()
    {
        Debug.DrawLine(transform.position, transform.position + velocity, Color.red, 0f);
        Debug.DrawLine(transform.position, transform.position + acceleration, Color.green, 0f);

    }

    private void OnDrawGizmosSelected()
    {
        DrawDebugs();
        Gizmos.DrawWireSphere(transform.position, maxRadious);
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
                float bias = Mathf.Lerp(maxRadious, 0, distance / 2);
                averageForce += direction * bias;
            }
        }
        averageForce /= boids.Count;
        this.acceleration += averageForce * separationStrenght;
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
}
