using System;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public static event Action onRotationStopped; 
    private RepairsManager repairsManager;

    public bool isRotating;
    private bool isDirectionReversed; 
    public float currentRotationSpeed;

    private void Start()
    {
        repairsManager = GetComponentInParent<RepairsManager>();
		currentRotationSpeed = RepairsConstants.StartingSpeed;
    }

    public void RotateWorkstation()
    {
        float zRotation = isDirectionReversed ? currentRotationSpeed
            : currentRotationSpeed * -1f;

        transform.eulerAngles += new Vector3(
            0f, 0f, zRotation * Time.deltaTime);
    }

    public void StartRotating()
    {
        isRotating = true;
        //repairsManager.ResetFeedbackText();
    }

    public void StopRotating()
    {
        isRotating = false;
        onRotationStopped?.Invoke();
    }

    // Increase the difficulty by decreasing the timing window 
    public void IncreaseRotationSpeed() =>
        currentRotationSpeed += RepairsConstants.SpeedIncrease;

    public void ResetRotationSpeed() =>
        currentRotationSpeed = RepairsConstants.StartingSpeed;

    // Increase the difficulty by disorientating the player 
    public void ReverseRotationDirection() =>
        isDirectionReversed = !isDirectionReversed;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isRotating)
            {
                StopRotating();
            }
            else
            {
                StartRotating();
            }
        }

        if (isRotating)
        {
            RotateWorkstation();
        }
    }
}