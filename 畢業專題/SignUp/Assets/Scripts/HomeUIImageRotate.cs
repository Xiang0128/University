using UnityEngine;
using System.Collections;

public class RotateUIImage : MonoBehaviour
{
    public float rotationSpeed = 2f;  // Speed of rotation
    public float rotationAmount = 90f; // How much to rotate (default: 90 degrees)
    public float minStartDelay = 0f;   // Minimum delay before starting
    public float maxStartDelay = 2f;   // Maximum delay before starting

    private float startAngle;  // Stores initial angle
    private float targetAngle; // Next rotation target
    private int rotationState = 0; // Tracks the current rotation step
    private bool canRotate = false; // Prevents movement before delay

    void Start()
    {
        startAngle = transform.eulerAngles.z; // Save initial rotation
        StartCoroutine(StartWithDelay());
    }

    void Update()
    {
        if (!canRotate) return; // Don't start rotating until delay is over

        // Smoothly rotate towards target rotation
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        // If close enough to target, set the next rotation
        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) < 1f)
        {
            SetNextRotation();
        }
    }

    IEnumerator StartWithDelay()
    {
        float delay = Random.Range(minStartDelay, maxStartDelay); // Pick a random delay (0 ~ 2 sec)
        yield return new WaitForSeconds(delay);
        canRotate = true; // Enable rotation after delay
        SetNextRotation();
    }

    void SetNextRotation()
    {
        switch (rotationState)
        {
            case 0: // Rotate +90
                targetAngle = startAngle + rotationAmount;
                rotationState = 1;
                break;
            case 1: // Rotate -90
                targetAngle = startAngle - rotationAmount;
                rotationState = 2;
                break;
            case 2: // Rotate -90 again
                targetAngle = startAngle - rotationAmount;
                rotationState = 3;
                break;
            case 3: // Rotate +90
                targetAngle = startAngle + rotationAmount;
                rotationState = 0;
                break;
        }
    }
}
