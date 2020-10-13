using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    public void PlayerWins()
    {
        repairsManager.points++;
        repairsManager.consecutiveWins++;
        IncreaseRotationSpeed();

        // Decrease green zone size every n wins 
        if (repairsManager.consecutiveWins % RepairsConstants.greenZoneShrinkInterval == 0 
            && repairsManager.consecutiveWins > 0)
        {
            greenZone.ReduceSize();
        }

        if (UnityEngine.Random.Range(0, RepairsConstants.rotationReversalUpperBound) > 
			RepairsConstants.rotationReversalThreshold)
        {
            ReverseRotationDirection();
        }

        repairsManager.UpdateFeedbackText(true);
    }

    public void PlayerLoses()
    {
        repairsManager.consecutiveWins = 0;
        ResetRotationSpeed();
        greenZone.ResetSize();
        repairsManager.UpdateFeedbackText(false);
    }

    // This will increase the difficulty by decreasing the timing window 
    public void IncreaseRotationSpeed()
    {
        rotationSpeed += RepairsConstants.speedIncrease;
    }

    private void ResetRotationSpeed()
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