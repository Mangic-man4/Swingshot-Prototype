using UnityEngine;

public class RunTimerTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Start,
        Finish
    }

    public TriggerType triggerType;

    private void OnTriggerEnter(Collider other)
    {
        RunTimer timer = other.GetComponent<RunTimer>();

        if (timer == null)
            return;

        if (triggerType == TriggerType.Start)
        {
            timer.StartRun();
        }
        else if (triggerType == TriggerType.Finish)
        {
            timer.FinishRun();
        }
    }
}