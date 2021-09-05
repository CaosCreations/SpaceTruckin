using System;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public static event Action OnRotationStarted;
    public static event Action OnRotationStopped;

    public bool IsRotating { get; private set; }
    public float CurrentRotationSpeed { get; private set; }
    private bool isDirectionReversed;

    private void Start()
    {
        CurrentRotationSpeed = RepairsConstants.StartingSpeed;
    }

    public void RotateWheel()
    {
        float zRotation = isDirectionReversed ? CurrentRotationSpeed
            : CurrentRotationSpeed * -1f;

        transform.eulerAngles += new Vector3(
            0f, 0f, zRotation * Time.deltaTime);
    }

    public void StartRotating()
    {
        IsRotating = true;
        OnRotationStopped.Invoke();
    }

    public void StopRotating(bool isResetting = false)
    {
        IsRotating = false;

        if (!isResetting)
        {
            OnRotationStopped.Invoke();
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

    public void ResetWheel()
    {
        StopRotating(isResetting: true);
        transform.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
        if (IsRotating)
        {
            RotateWheel();
        }
    }
}
