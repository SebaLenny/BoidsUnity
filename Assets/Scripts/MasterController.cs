using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : Singleton<MasterController>
{
    private int rulesCount = Generation.generationSize;
    public List<RuleSet> rules;
    public GeneticAlgorithm geneticAlgorithm;
    public Transform spawnPoint;
    public Course course;
    public GameObject boidPrefab;

    protected override void Awake()
    {
        base.Awake();
        geneticAlgorithm = new GeneticAlgorithm();
        geneticAlgorithm.GenerateRandomGeneration();
        CreateRules();
        ReadLasGeneration();
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

    private void ReadLasGeneration()
    {
        Generation gen = geneticAlgorithm.GetLastGeneration();
        for (int i = 0; i < rulesCount; i++)
        {
            rules[i].FieldsListNormalized = gen.population[i].genes;
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
