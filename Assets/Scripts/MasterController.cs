using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : Singleton<MasterController>
{
    public int rulesCount = 5;
    public List<RuleSet> rules;
    public Transform spawnPoint;
    public Course course;
    public GameObject boidPrefab;

    protected override void Awake()
    {
        base.Awake();
        CreateRules();
        if (spawnPoint == null) spawnPoint = (new GameObject("SpawnPoint")).transform;
    }

    private void Start()
    {
        SpawnGroups();
    }

    private void CreateRules()
    {
        rules = new List<RuleSet>();
        for (int i = 0; i < rulesCount; i++)
        {
            rules.Add(new RuleSet(spawnPoint, course));
        }
    }

    private void SpawnGroups()
    {
        for (int r = 0; r < rules.Count; r++)
        {
            rules[r].SpawnBoids(r / (float)rules.Count, boidPrefab);
        }
    }
}
