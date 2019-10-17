using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoidController : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 acceleration;
    public Vector3 startVelocity;
    public Vector3 startAcceleration;
    public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
    public Vector3 Acceleration { get { return acceleration; } set { velocity = value; } }
    public float maxVelocity;
    public float maxRadious;

    private void Start()
    {
        velocity = startVelocity;
        acceleration = startAcceleration;
    }
    private void Update()
    {
        DrawDebugs();
    }

    private void FixedUpdate()
    {
        AdvanceSimulation();
    }

    public void AdvanceSimulation()
    {
        Collider[] referencedBoidsColliders = Physics.OverlapSphere(transform.position, maxRadious); // use non aloc and try to settle for masks for boids separately
        List<BoidController> boids = referencedBoidsColliders.Select(c => c.GetComponent<BoidController>()).ToList();
        velocity += acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        transform.position += velocity * Time.fixedDeltaTime;
        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
    }

    public void DrawDebugs()
    {
        Debug.DrawLine(transform.position, transform.position + velocity, Color.red, 0f);
        Debug.DrawLine(transform.position, transform.position + acceleration, Color.green, 0f);
    }

    public void ApplySeparation()
    {

    }

    public void ApplyAligment()
    {

    }

    public void ApplyCohesion()
    {

    }
}
