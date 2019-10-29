using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MasterController : Singleton<MasterController>
{
    public Vector3 bounds;
    [Range(0f, 5f)]
    public float reaccelerationForce = 1f;
    [Range(0f, 180f)]
    public float angleThreshold = 90f;
    public bool aligment;
    [Range(0f, 5f)]
    public float aligmentStrenght = 1;
    public bool separation;
    [Range(0f, 5f)]
    public float separationStrenght = 1;
    public bool cohesion;
    [Range(0f, 5f)]
    public float cohesionStrenght = 1;
    public bool collision;
    [Range(0f, 5f)]
    public float collisionStrenght = 1;
    public bool chaseTarget = false;
    [Range(0f, 5f)]
    public float targetStrenght = 1;
    public GameObject target;
    public int samplePointsCount = 100;
    private static List<Vector3> pointsOnSphere;
    public List<Vector3> PointsOnSphere { get { return pointsOnSphere; } }

    protected override void Awake()
    {
        base.Awake();
        GeneratePoints();
    }

    private void Update()
    {
        // foreach (var point in pointsOnSphere)
        // {
        //     Debug.DrawLine(Vector3.zero, point);
        // }
    }

    /// Reference https://stackoverflow.com/a/44164075
    private void GeneratePoints()
    {
        pointsOnSphere = new List<Vector3>();
        float thetaBase = Mathf.PI * (1 + Mathf.Sqrt(5));
        for (int i = 0; i < samplePointsCount; i++)
        {
            float k = i + .5f;
            float phi = Mathf.Acos(1 - 2 * k / samplePointsCount);
            float theta = thetaBase * k;
            float sinPhi = Mathf.Sin(phi);
            pointsOnSphere.Add(new Vector3(
                Mathf.Cos(theta) * sinPhi,
                Mathf.Sin(theta) * sinPhi,
                Mathf.Cos(phi)
            ));
        }
    }
}
