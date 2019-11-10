[System.Serializable]
public class RuleSet
{
    public int boidsCount = 10;
    public float maxVelocity = 5;
    public float maxAcceleration = 1;
    public float seeAngle = 180;
    public RuleParameters aligment;
    public RuleParameters separation;
    public RuleParameters cohesion;
    public RuleParameters collisionAvoidance;
    public RuleParameters targetChasing;
}
