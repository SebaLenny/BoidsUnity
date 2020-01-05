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
        RandomiseRules();
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

    private void RandomiseRules()
    {
        foreach (var rule in rules)
        {
            var norm = rule.FieldsListNormalized;
            for (int i = 0; i < norm.Count; i++)
            {
                norm[i] += RandomFromDistribution.RandomRangeNormalDistribution(-.5f, .5f, RandomFromDistribution.ConfidenceLevel_e._999);
                norm[i] = Mathf.Clamp01(norm[i]);
            }
            rule.FieldsListNormalized = norm;
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
