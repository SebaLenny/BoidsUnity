using UnityEngine;

[RequireComponent(typeof(TargetManager))]
[RequireComponent(typeof(BoidController))]
public class ScoreUpcaster : MonoBehaviour
{
    private TargetManager tm;
    private BoidController bc;
    private int lastCheck;

    private void Awake()
    {
        tm = GetComponent<TargetManager>();
        bc = GetComponent<BoidController>();
        tm.Checkpoint += ReportScore;
        SetClock();
    }

    private void ReportScore()
    {
        bc.ruleSet.timeScore += 1f / (Time.frameCount - lastCheck);
        SetClock();
    }
    public void ReportCollision(float magnitude)
    {
        bc.ruleSet.collisionScore += magnitude;
    }

    private void SetClock()
    {
        lastCheck = Time.frameCount;
    }
}