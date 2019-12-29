using UnityEngine;

public class SimulationScaler : MonoBehaviour
{
    [Range(0.0f, 16f)]
    public float simulationTimeScale = 1f;

    private void Update()
    {
        SetSimulationTime(simulationTimeScale);
    }

    private void SetSimulationTime(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.01f * scale;
    }
}