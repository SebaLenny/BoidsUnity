using System.Collections.Generic;

public class GeneticAlgorithm
{
    public List<Generation> generations = new List<Generation>();

    public void GenerateGeneration()
    {
        generations.Add(new Generation());
    }
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
}
