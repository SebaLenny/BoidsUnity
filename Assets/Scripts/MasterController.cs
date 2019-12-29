using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : Singleton<MasterController>
{
    public List<RuleSet> rules;
    public GameObject boidPrefab;

    protected override void Awake()
    {
        base.Awake();
        CreateMissingSpawnpoints();
    }

    private void Start()
    {
        SpawnGroups();
    }

    private void OnValidate()
    {
        CreateMissingSpawnpoints();
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
}
