using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
public class GeneticAlgorithm
{
    public readonly static float mutationChance = 0.05f;
    public readonly static int generationSize = 5;
    public readonly static int chromosomeSize = 10;
    public List<Generation> generations = new List<Generation>();
    private static string dir = $@"GenerationsOutput{DateTime.Now.Year}.{String.Format("{0:00}", DateTime.Now.Month)}.{String.Format("{0:00}", DateTime.Now.Day)}.{String.Format("{0:00}", DateTime.Now.Hour)}.{String.Format("{0:00}", DateTime.Now.Minute)}";
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
        var time = System.DateTime.Now;
        Directory.CreateDirectory(dir);
        if (!File.Exists(dir + "/main.py"))
            File.Copy("main.py", dir + "/main.py");
        File.WriteAllText(dir + $@"/gen{generations.Count}.json", JsonConvert.SerializeObject(GetLastGeneration()));
        generations.Add(GetLastGeneration().GenerateNextGeneration());
    }
}
