using UnityEngine;

public class MovementMeasurements : MonoBehaviour
{
    private bool jumpActive;
    private float jumpStartTime;
    private float jumpStartY;
    private float highestY;

    private bool runActive;
    private float runStartTime;

    void Update()
    {
        // Start measuring a jump when Space is pressed.
        if (Input.GetKeyDown(KeyCode.Space) && !jumpActive)
        {
            jumpActive = true;
            jumpStartTime = Time.time;
            jumpStartY = transform.position.y;
            highestY = jumpStartY;

            Debug.Log("Jump measurement started.");
        }

        if (jumpActive)
        {
            if (transform.position.y > highestY)
                highestY = transform.position.y;

            // Assumes you've returned roughly to your starting height.
            if (Time.time > jumpStartTime + 0.1f &&
                transform.position.y <= jumpStartY + 0.01f)
            {
                float airtime = Time.time - jumpStartTime;
                float jumpHeight = highestY - jumpStartY;

                Debug.Log(
                    $"Jump Height: {jumpHeight:F3} m | " +
                    $"Airtime: {airtime:F3} s"
                );

                jumpActive = false;
            }
        }

        // F1 starts/stops the level timer.
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!runActive)
            {
                runActive = true;
                runStartTime = Time.time;
                Debug.Log("Run timer started.");
            }
            else
            {
                float runTime = Time.time - runStartTime;
                runActive = false;

                Debug.Log($"Run Time: {runTime:F3} s");
            }
        }
    }
}