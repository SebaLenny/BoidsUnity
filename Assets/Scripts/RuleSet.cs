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
    public RuleParameters aligment;
    public RuleParameters separation;
    public RuleParameters cohesion;
    public RuleParameters collisionAvoidance;
    public RuleParameters targetChasing;
    public Course course;
    public float score = 0f;
    private List<float> fieldsList = null;
    public List<float> FieldsList
    {
        get { return fieldsList ?? GenerateFieldsList(); }
        set { SetFields(value); }
    }

    private void SetFields(List<float> value)
    {
        seeAngle = value[0];
        aligment.range = value[1];
        aligment.strenght = value[2];
        separation.range = value[3];
        separation.strenght = value[4];
        cohesion.range = value[5];
        cohesion.strenght = value[6];
        collisionAvoidance.range = value[7];
        collisionAvoidance.strenght = value[8];
        targetChasing.strenght = value[9];
        fieldsList = value;
    }

    private List<float> GenerateFieldsList()
    {
        fieldsList = new List<float>();
        fieldsList.Add(seeAngle);
        fieldsList.Add(aligment.range);
        fieldsList.Add(aligment.strenght);
        fieldsList.Add(separation.range);
        fieldsList.Add(separation.strenght);
        fieldsList.Add(cohesion.range);
        fieldsList.Add(cohesion.strenght);
        fieldsList.Add(collisionAvoidance.range);
        fieldsList.Add(collisionAvoidance.strenght);
        fieldsList.Add(targetChasing.strenght);
        return fieldsList;
    }


}
