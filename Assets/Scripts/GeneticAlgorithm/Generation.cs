using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generation
{
    public readonly static int generationSize = 8;
    public List<Chromosome> population = new List<Chromosome>();

    public void LoadGeneration(List<RuleSet> rules)
    {
        population = new List<Chromosome>();
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

    public Generation GenerateNextGeneration()
    {
        var toRet = new Generation();
        var orderedPopulation = population.OrderByDescending(c => c.timeScore / (c.collisionScore + 1f)).Select(c => c.GenesCopy()).ToList();

        // selection
        var pruned = Selection(orderedPopulation);

        // crossover
        toRet.population = new List<Chromosome>(pruned);
        while (toRet.population.Count != orderedPopulation.Count)
        {
            Chromosome child1, child2;
            (child1, child2) = Breed(pruned[Random.Range(0, pruned.Count)], pruned[Random.Range(0, pruned.Count)]);
            toRet.population.Add(child1);
            if (orderedPopulation.Count - toRet.population.Count > 1)
            {
                toRet.population.Add(child2);
            }
        }

        // mutation
        foreach (var chromosome in toRet.population)
        {
            chromosome.Mutate();
        }

        foreach (var chromosome in toRet.population)
        {
            chromosome.ClearScore();
        }

        return toRet;
    }

    public List<Chromosome> Selection(List<Chromosome> orderedPopulation)
    {
        var pruned = new List<Chromosome>();
        for (int i = 0; i < orderedPopulation.Count; i++)
        {
            if (Random.value > i / (float)orderedPopulation.Count)
            {
                pruned.Add(orderedPopulation[i]);
            }
        }
        return pruned;
    }

    public (Chromosome, Chromosome) Breed(Chromosome partent1, Chromosome partent2)
    {
        int crossoverPoint = Random.Range(0, Chromosome.chromosomeSize);
        var child1 = new Chromosome();
        var child2 = new Chromosome();
        child1.genes = partent1.genes.Take(crossoverPoint).Union(partent2.genes.Skip(crossoverPoint)).ToList();
        child2.genes = partent2.genes.Take(crossoverPoint).Union(partent1.genes.Skip(crossoverPoint)).ToList();
        return (child1, child2);
    }

    public List<Chromosome> FillUpWithCrossover(List<Chromosome> population)
    {
        return null;
    }
}
