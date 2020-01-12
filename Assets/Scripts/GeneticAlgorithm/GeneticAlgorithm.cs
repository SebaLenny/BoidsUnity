using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
public class GeneticAlgorithm
{
    public readonly static float mutationChance = 0.05f;
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
        File.WriteAllText($"gen{generations.Count}.json", JsonConvert.SerializeObject(GetLastGeneration()));
        generations.Add(GetLastGeneration().GenerateNextGeneration());
    }
}
