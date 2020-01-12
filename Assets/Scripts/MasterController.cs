using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SimulationState
{
    Running,
    Finished
}

public class MasterController : Singleton<MasterController>
{
    private int rulesCount = Generation.generationSize;
    public List<RuleSet> rules;
    public GeneticAlgorithm geneticAlgorithm;
    public Transform spawnPoint;
    public Course course;
    public GameObject boidPrefab;
    private SimulationState simulationState = SimulationState.Running;
    public event Action Disposed = delegate { };

    protected override void Awake()
    {
        base.Awake();
        InitSimulation();
        if (spawnPoint == null) spawnPoint = (new GameObject("SpawnPoint")).transform;
    }

    private void Start()
    {
        SpawnGroups();
    }

    private void InitSimulation()
    {
        geneticAlgorithm = new GeneticAlgorithm();
        geneticAlgorithm.GenerateRandomGeneration();
        CreateRules();
        ReadLasGeneration();
        course.Finished += FirstBoidFinished;
        Disposed += NextGeneration;
    }

    private void NextGeneration()
    {
        geneticAlgorithm.FetchGeneration(rules);
        geneticAlgorithm.GenerateNextGeneration();
        ReadLasGeneration();
        SpawnGroups();
        simulationState = SimulationState.Running;
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
            rules[i].timeScore = gen.population[i].timeScore;
            rules[i].collisionScore = gen.population[i].collisionScore;
        }
    }

    private void SpawnGroups()
    {
        for (int r = 0; r < rules.Count; r++)
        {
            rules[r].SpawnBoids(r / (float)rules.Count, boidPrefab);
        }
    }

    private void FirstBoidFinished()
    {
        if (simulationState == SimulationState.Finished) return;
        simulationState = SimulationState.Finished;
        StartCoroutine(DisposeBoidsAfter(5f));
    }

    private IEnumerator DisposeBoidsAfter(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        foreach (var rule in rules)
        {
            rule.DisposeAllBoids();
        }
        Disposed();
    }
}
