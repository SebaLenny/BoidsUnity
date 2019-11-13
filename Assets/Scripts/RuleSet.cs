using UnityEngine;

[System.Serializable]
public class RuleSet
{
    public Transform spawnPoint;
    [Range(0, 30)]
    public int boidsCount = 10;
    [Range(0f, 30f)]
    public float maxVelocity = 5;
    [Range(0f, 5f)]
    public float maxAcceleration = 1;
    [Range(0f, 180f)]
    public float seeAngle = 180;
    public RuleParameters aligment;
    public RuleParameters separation;
    public RuleParameters cohesion;
    public RuleParameters collisionAvoidance;
    public RuleParameters targetChasing;
}
