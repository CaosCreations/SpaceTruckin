using System;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public static event Action onRotationStopped; 
    public GreenZone greenZone;
    public RepairsManager repairsManager;
        
    public bool isRotating;
    private float rotationSpeed;

    private void Start()
    {
		rotationSpeed = RepairsConstants.startingSpeed;
    }

    public void RotateWorkstation()
    {
        transform.eulerAngles += new Vector3(0f, 0f, rotationSpeed);
    }

    public void StartRotating()
    {
        isRotating = true;
        repairsManager.ResetFeedbackText();
    }

    public void StopRotating()
    {
        isRotating = false;
        onRotationStopped?.Invoke();
    }

    // This will increase the difficulty by decreasing the timing window 
    public void IncreaseRotationSpeed()
    {
        rotationSpeed += RepairsConstants.speedIncrease;
    }

    public void ResetRotationSpeed()
    {
        rotationSpeed = RepairsConstants.startingSpeed;
    }

    // This will increase the difficulty by disorientating the player 
    public void ReverseRotationDirection()
    {
        rotationSpeed *= -1; 
    }

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