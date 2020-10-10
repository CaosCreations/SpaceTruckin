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
    private float startingSpeed; 
    private float speedIncrease;

    private float rotationReversalThreshold;
    private float rotationReversalUpperBound;
    private float greenZoneShrinkInterval;

    private void Start()
    {
        startingSpeed = 3f;
        rotationSpeed = startingSpeed;
        speedIncrease = 1f;

        rotationReversalThreshold = 3;
        rotationReversalUpperBound = 5;
        greenZoneShrinkInterval = 3; 
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
        if (repairsManager.consecutiveWins % greenZoneShrinkInterval == 0 
            && repairsManager.consecutiveWins > 0)
        {
            greenZone.ReduceSize();
        }

        if (UnityEngine.Random.Range(0, rotationReversalUpperBound) > rotationReversalThreshold)
        {
            ReverseRotationDirection();
        }

        repairsManager.UpdateText(true);
    }

    public void PlayerLoses()
    {
        repairsManager.consecutiveWins = 0;
        ResetRotationSpeed();
        greenZone.ResetSize();
        repairsManager.UpdateText(false);
    }

    // This will increase the difficulty by decreasing the timing window 
    public void IncreaseRotationSpeed()
    {
        rotationSpeed += speedIncrease;
    }

    private void ResetRotationSpeed()
    {
        rotationSpeed = startingSpeed;
    }

    // This will increase the difficulty by disorientating the player 
    public void ReverseRotationDirection()
    {
        rotationSpeed *= -1; 
        Debug.Log("Speed: " + rotationSpeed);
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
