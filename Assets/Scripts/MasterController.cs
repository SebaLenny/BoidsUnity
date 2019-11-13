using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MasterController : Singleton<MasterController>
{
    public List<RuleSet> rules;
    public Vector3 bounds;
    public GameObject boidPrefab;
    private int samplePointsCount = 100;
    private static List<Vector3> pointsOnSphere;
    public List<Vector3> PointsOnSphere { get { return pointsOnSphere; } }

    protected override void Awake()
    {
        base.Awake();
        GeneratePoints();
    }

    private void Start()
    {
        SpawnGroups();
    }

    private void SpawnGroups()
    {
        for (int r = 0; r < rules.Count; r++)
        {
            Color col = Color.HSVToRGB(r / (float)rules.Count, 1, 1);
            for (int i = 0; i < rules[r].boidsCount; i++)
            {
                GameObject dummy = Instantiate(boidPrefab, rules[r].spawnPoint.position, Quaternion.identity);
                dummy.GetComponent<BoidController>().ruleSet = rules[r];
                dummy.GetComponentInChildren<Renderer>().material.color = col;
            }
        }
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
