using System;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public static event Action onRotationStopped; 
    private RepairsManager repairsManager;
        
    public bool isRotating;
    private bool isDirectionReversed; 
    public float rotationSpeed;

    private void Start()
    {
        repairsManager = GetComponentInParent<RepairsManager>();
		rotationSpeed = RepairsConstants.startingSpeed;
    }

    public void RotateWorkstation()
    {
        float zRotation = isDirectionReversed ? rotationSpeed
            : rotationSpeed * -1f;

        transform.eulerAngles += new Vector3(
            0f, 0f, zRotation * Time.deltaTime);
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

    // Increase the difficulty by decreasing the timing window 
    public void IncreaseRotationSpeed() =>
        rotationSpeed += RepairsConstants.speedIncrease;

    public void ResetRotationSpeed() =>
        rotationSpeed = RepairsConstants.startingSpeed;

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