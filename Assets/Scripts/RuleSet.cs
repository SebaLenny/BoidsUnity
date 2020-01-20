using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuleSet
{
    public Transform spawnPoint;
    [Range(0, 30)]
    public int boidsCount = 5;
    [Range(0f, 30f)]
    public float maxVelocity = 5;
    [Range(0f, 20f)]
    public float maxAcceleration = 5;
    [Range(0f, 180f)]
    public float seeAngle = 180;
    public RuleParameters alignment;
    public RuleParameters separation;
    public RuleParameters cohesion;
    public RuleParameters collisionAvoidance;
    public RuleParameters targetChasing;
    public Course course;
    public float timeScore = 0f;
    public float collisionScore = 0f;
    public List<GameObject> boids;

    public RuleSet(Transform spawnPoint, Course course)
    {
        boids = new List<GameObject>();
        alignment = new RuleParameters();
        separation = new RuleParameters { considerOtherGroups = true };
        cohesion = new RuleParameters();
        collisionAvoidance = new RuleParameters();
        targetChasing = new RuleParameters();
        this.spawnPoint = spawnPoint;
        this.course = course;
    }

    public void SpawnBoids(float colour, GameObject boidPrefab)
    {
        Color col = Color.HSVToRGB(colour, 1, 1);
        Color hdr = Color.HSVToRGB(colour, 1, .35f);
        for (int i = 0; i < boidsCount; i++)
        {
            var randomOffset = UnityEngine.Random.insideUnitSphere * 10f;
            GameObject boid = GameObject.Instantiate(boidPrefab, spawnPoint.position + randomOffset, Quaternion.identity);
            boid.GetComponent<BoidController>().ruleSet = this;
            boid.GetComponentInChildren<Renderer>().material.color = col;
            boid.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", hdr);
            boids.Add(boid);
        }
    }

    public List<float> FieldsList
    {
        get { return GenerateFieldsList(); }
        set { SetFields(value); }
    }

    public List<float> FieldsListNormalized
    {
        get { return GenerateFieldsListNormalized(); }
        set { SetFieldsNormalized(value); }
    }

    private void SetFields(List<float> value)
    {
        seeAngle = value[0];
        alignment.range = value[1];
        alignment.strength = value[2];
        separation.range = value[3];
        separation.strength = value[4];
        cohesion.range = value[5];
        cohesion.strength = value[6];
        collisionAvoidance.range = value[7];
        collisionAvoidance.strength = value[8];
        targetChasing.strength = value[9];
    }

    private List<float> GenerateFieldsList()
    {
        var fieldsList = new List<float>();
        fieldsList.Add(seeAngle);
        fieldsList.Add(alignment.range);
        fieldsList.Add(alignment.strength);
        fieldsList.Add(separation.range);
        fieldsList.Add(separation.strength);
        fieldsList.Add(cohesion.range);
        fieldsList.Add(cohesion.strength);
        fieldsList.Add(collisionAvoidance.range);
        fieldsList.Add(collisionAvoidance.strength);
        fieldsList.Add(targetChasing.strength);
        return fieldsList;
    }

    private void SetFieldsNormalized(List<float> value)
    {
        seeAngle = value[0].Map(0, 1, 0, 180);
        alignment.range = value[1].Map(0, 1, 0, 15);
        alignment.strength = value[2].Map(0, 1, 0, 5);
        separation.range = value[3].Map(0, 1, 0, 15);
        separation.strength = value[4].Map(0, 1, 0, 5);
        cohesion.range = value[5].Map(0, 1, 0, 15);
        cohesion.strength = value[6].Map(0, 1, 0, 5);
        collisionAvoidance.range = value[7].Map(0, 1, 0, 15);
        collisionAvoidance.strength = value[8].Map(0, 1, 0, 5);
        targetChasing.strength = value[9].Map(0, 1, 0, 5);
    }

    private List<float> GenerateFieldsListNormalized()
    {
        var fieldsList = new List<float>();
        fieldsList.Add(seeAngle.Map(0, 180, 0, 1));
        fieldsList.Add(alignment.range.Map(0, 15, 0, 1));
        fieldsList.Add(alignment.strength.Map(0, 5, 0, 1));
        fieldsList.Add(separation.range.Map(0, 15, 0, 1));
        fieldsList.Add(separation.strength.Map(0, 5, 0, 1));
        fieldsList.Add(cohesion.range.Map(0, 15, 0, 1));
        fieldsList.Add(cohesion.strength.Map(0, 5, 0, 1));
        fieldsList.Add(collisionAvoidance.range.Map(0, 15, 0, 1));
        fieldsList.Add(collisionAvoidance.strength.Map(0, 5, 0, 1));
        fieldsList.Add(targetChasing.strength.Map(0, 5, 0, 1));
        return fieldsList;
    }

    public void DisposeAllBoids()
    {
        foreach (var boid in boids)
        {
            GameObject.DestroyImmediate(boid);
        }
        boids = new List<GameObject>();
    }
}