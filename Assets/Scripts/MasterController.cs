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

    private void OnValidate()
    {
        CreateMissingSpawnpoints();
    }

    private void CreateMissingSpawnpoints()
    {
        foreach (var rule in rules)
        {
            if (rule.spawnPoint == null)
            {
                GameObject dummy = new GameObject("SpawnPoint" + rules.IndexOf(rule));
                dummy.transform.parent = transform;
                rule.spawnPoint = dummy.transform;
            }
        }
    }

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
            Color hdr = Color.HSVToRGB(r / (float)rules.Count, 1, .35f);
            for (int i = 0; i < rules[r].boidsCount; i++)
            {
                GameObject dummy = Instantiate(boidPrefab, rules[r].spawnPoint.position, Quaternion.identity);
                dummy.GetComponent<BoidController>().ruleSet = rules[r];
                dummy.GetComponentInChildren<Renderer>().material.color = col;
                dummy.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", hdr);
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
