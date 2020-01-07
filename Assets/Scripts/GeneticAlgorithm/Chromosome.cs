using System.Collections.Generic;
using UnityEngine;

public class Chromosome
{
    public readonly static int chromosomeSize = 10;
    public List<float> genes = new List<float>();
    public float timeScore = 0f;
    public float collisionScore = 0f;

    public static Chromosome GenerateRandomChromosome()
    {
        var Dummy = new Chromosome();
        for (int i = 0; i < chromosomeSize; i++)
        {
            Dummy.genes.Add(Random.Range(0f, 1f));
        }
        return Dummy;
    }

    public static Chromosome GenerateRandomNormalChromosome()
    {
        var Dummy = new Chromosome();
        for (int i = 0; i < chromosomeSize; i++)
        {
            Dummy.genes.Add(RandomFromDistribution.RandomRangeNormalDistribution(0, 1, RandomFromDistribution.ConfidenceLevel_e._999));
        }
        return Dummy;
    }

    public Chromosome GenesCopy()
    {
        return new Chromosome
        {
            genes = new List<float>(this.genes),
            timeScore = this.timeScore,
            collisionScore = this.collisionScore
        };
    }

    public void Mutate()
    {
        for (int i = 0; i < genes.Count; i++)
        {
            genes[i] += RandomFromDistribution.RandomRangeNormalDistribution(-.125f, .125f, RandomFromDistribution.ConfidenceLevel_e._999);
            genes[i] = Mathf.Clamp(genes[i], 0f, 1f);
        }
    }

    public void ClearScore()
    {
        timeScore = 0f;
        collisionScore = 0f;
    }
}
