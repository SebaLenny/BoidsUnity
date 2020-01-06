using System.Collections.Generic;

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
            Dummy.genes.Add(RandomFromDistribution.RandomRangeNormalDistribution(0, 1, RandomFromDistribution.ConfidenceLevel_e._999));
        }
        return Dummy;
    }
}
