using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoidController : MonoBehaviour
{
    private Vector3 direction; // for reading only
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
    // [Range(0f, 5f)]
    // public float aligmentStrenght = .1f;
    // [Range(0f, 5f)]
    // public float separationStrenght = .5f;
    // [Range(0f, 5f)]
    // public float cohesionStrenght = 1f;
    [Range(0f, 5f)]
    public float reaccelerationForce = 1f;
    public bool observe = false;
    private void Start()
    {
        velocity = startVelocity + new Vector3(Random.Range(-randomStartingVelocity, randomStartingVelocity), Random.Range(-randomStartingVelocity, randomStartingVelocity), Random.Range(-randomStartingVelocity, randomStartingVelocity));
        direction = velocity.normalized;
    }

    private void FixedUpdate()
    {
        AdvanceSimulation();
        ApplyBounds();
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

    public void AdvanceSimulation()
    {
        List<BoidController> boids = getNerbyBoids();
        if (MasterController.Instance.separation)
            ApplySeparation(boids);
        if (MasterController.Instance.aligment)
            ApplyAligment(boids);
        if (MasterController.Instance.cohesion)
            ApplyCohesion(boids);

        ApplyVectors();
        if (observe)
        {
            DrawDebugs();
        }
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
        int count = 0;
        foreach (var boid in boids)
        {
            if (isSeeing(boid))
            {
                Vector3 diff = transform.position - boid.transform.position;
                float dist = diff.magnitude + 0.01f;
                diff /= dist;
                averageForce += diff;
                count++;
            }
        }
        if (count != 0)
            averageForce /= count;
        averageForce = averageForce.normalized * maxVelocity;
        this.acceleration += averageForce * MasterController.Instance.separationStrenght;// * 10;
    }

    public void ApplyAligment(List<BoidController> boids)
    {
        if (boids.Count == 0) return;
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
        averageVelocity = averageVelocity.normalized * maxVelocity;
        this.acceleration += (averageVelocity - velocity) * MasterController.Instance.aligmentStrenght;
    }

    public void ApplyCohesion(List<BoidController> boids)
    {
        if (boids.Count == 0) return;
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
        this.acceleration += (averagePosition - transform.position) * MasterController.Instance.cohesionStrenght;
    }

    public bool isSeeing(BoidController otherBoid)
    {
        return Vector3.Angle(velocity, otherBoid.transform.position - transform.position) < angleThreshold;
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
            //Gizmos.DrawWireSphere(transform.position, maxRadious);
        }
    }
}
