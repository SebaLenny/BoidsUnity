using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generation
{
    
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
        for (int i = 0; i < GeneticAlgorithm.generationSize; i++)
        {
            dummy.population.Add(Chromosome.GenerateRandomChromosome());
        }
        return dummy;
    }

    public Generation GenerateNextGeneration()
    {
        var toRet = new Generation();

        var orderedPopulation = population.OrderByDescending(c => c.timeScore / (c.collisionScore + 1f)).Select(c => c.GenesCopy()).ToList();
        DebugPopulation(orderedPopulation);

        // selection
        var pruned = Selection(orderedPopulation);

        // crossover
        toRet.population = new List<Chromosome>(pruned);
        while (toRet.population.Count != orderedPopulation.Count)
        {
            Chromosome child1, child2;
            (child1, child2) = Breed(pruned[UnityEngine.Random.Range(0, pruned.Count)], pruned[UnityEngine.Random.Range(0, pruned.Count)]);
            toRet.population.Add(child1);
            if (orderedPopulation.Count - toRet.population.Count > 1)
            {
                toRet.population.Add(child2);
            }
        }

        // mutation
        foreach (var chromosome in toRet.population)
        {
            chromosome.Mutate(GeneticAlgorithm.mutationChance);
        }

        foreach (var chromosome in toRet.population)
        {
            chromosome.ClearScore();
        }

        return toRet;
    }

    private void DebugPopulation(List<Chromosome> orderedPopulation)
    {
        Debug.Log("----\n");
        Debug.Log($"Best chromosome: {orderedPopulation[0].timeScore / (1 + orderedPopulation[0].collisionScore)}" +
        $"   time score: {orderedPopulation[0].timeScore}" +
        $"   collision score: {orderedPopulation[0].collisionScore},");
        Debug.Log($"Worst chromosome: {orderedPopulation[orderedPopulation.Count - 1].timeScore / (1 + orderedPopulation[orderedPopulation.Count - 1].collisionScore)}" +
        $"   time score: {orderedPopulation[orderedPopulation.Count - 1].timeScore}" +
        $"   collision score: {orderedPopulation[orderedPopulation.Count - 1].collisionScore},");
    }

    public List<Chromosome> Selection(List<Chromosome> orderedPopulation)
    {
        var pruned = new List<Chromosome>();
        for (int i = 0; i < orderedPopulation.Count; i++)
        {
            if (UnityEngine.Random.value > i / (float)orderedPopulation.Count)
            {
                pruned.Add(orderedPopulation[i]);
            }
        }
        return pruned;
    }

    public (Chromosome, Chromosome) Breed(Chromosome partent1, Chromosome partent2)
    {
        int crossoverPoint = UnityEngine.Random.Range(0, GeneticAlgorithm.chromosomeSize);
        var child1 = new Chromosome();
        var child2 = new Chromosome();
        child1.genes = partent1.genes.Take(crossoverPoint).Concat(partent2.genes.Skip(crossoverPoint)).ToList();
        child2.genes = partent2.genes.Take(crossoverPoint).Concat(partent1.genes.Skip(crossoverPoint)).ToList();
        return (child1, child2);
    }

    public List<Chromosome> FillUpWithCrossover(List<Chromosome> population)
    {
        return null;
    }
}
