using System;
using System.Collections.Generic;

public class GeneticAlgorithm
{
    public List<Generation> generations = new List<Generation>();
    public void GenerateRandomGeneration()
    {
        generations.Add(Generation.GenerateRandomGeneration());
    }

    public Generation GetLastGeneration()
    {
        return generations[generations.Count - 1] ?? null;
    }

    public void FetchGeneration(List<RuleSet> rules)
    {
        GetLastGeneration().LoadGeneration(rules);
    }

    public void GenerateNextGeneration()
    {
        generations.Add(GetLastGeneration().GenerateNextGeneration());
    }
}