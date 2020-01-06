using System.Collections.Generic;

public class Generation
{
    public readonly static int generationSize = 5;
    public List<Chromosome> population = new List<Chromosome>();

    public void LoadGeneration(List<RuleSet> rules)
    {
        foreach (var rule in rules)
        {
            population.Add(
                new Chromosome
                {
                    genes = rule.FieldsListNormalized,
                    collisionScore = rule.collisionScore,
                    timeScore = rule.timeScore
                }
            );
        }
    }

    public static Generation GenerateRandomGeneration()
    {
        var dummy = new Generation();
        for (int i = 0; i < generationSize; i++)
        {
            dummy.population.Add(Chromosome.GenerateRandomChromosome());
        }
        return dummy;
    }
    ///TODO: FINISH THIS
    public Generation GenerateNextGeneration()
    {
        return GenerateRandomGeneration();
    }
}
