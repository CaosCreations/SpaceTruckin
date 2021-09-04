using System;
using UnityEngine;

public class Workstation : MonoBehaviour
{
    public static event Action OnRotationStoppedByPlayer;

    public bool IsRotating { get; private set; }
    public float CurrentRotationSpeed { get; private set; }
    private bool isDirectionReversed; 

    private void Start()
    {
        CurrentRotationSpeed = RepairsConstants.StartingSpeed;
    }

    public void RotateWorkstation()
    {
        float zRotation = isDirectionReversed ? CurrentRotationSpeed
            : CurrentRotationSpeed * -1f;

        transform.eulerAngles += new Vector3(
            0f, 0f, zRotation * Time.deltaTime);
    }

    public void StartRotating()
    {
        IsRotating = true;
    }

    public void StopRotating(bool isResetting = false)
    {
        IsRotating = false;

        if (!isResetting)
        {
            OnRotationStoppedByPlayer?.Invoke();
        }
    }

    // Increase the difficulty by decreasing the timing window 
    public void IncreaseRotationSpeed() =>
        CurrentRotationSpeed += RepairsConstants.SpeedIncrease;

    public void ResetRotationSpeed() =>
        CurrentRotationSpeed = RepairsConstants.StartingSpeed;

    // Increase the difficulty by disorientating the player 
    public void ReverseRotationDirection() =>
        isDirectionReversed = !isDirectionReversed;

    public void ResetWorkstation()
    {
        StopRotating(isResetting: true);
        transform.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
        if (IsRotating)
        {
            RotateWorkstation();
        }
    }
}