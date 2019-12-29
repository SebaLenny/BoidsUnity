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
        bc.ruleSet.score += 1f / (Time.frameCount - lastCheck);
        SetClock();
        Debug.Log(bc.ruleSet.score);
    }

    private void SetClock()
    {
        lastCheck = Time.frameCount;
    }
}