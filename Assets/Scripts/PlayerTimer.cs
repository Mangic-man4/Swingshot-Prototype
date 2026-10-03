using UnityEngine;

public class RunTimer : MonoBehaviour
{
    private bool runActive;
    private float runStartTime;

    public void StartRun()
    {
        runStartTime = Time.time;
        runActive = true;

        Debug.Log("Run started.");
    }

    public void FinishRun()
    {
        if (!runActive)
            return;

        float runTime = Time.time - runStartTime;
        runActive = false;

        Debug.Log($"Run Time: {runTime:F3} s");
    }
}