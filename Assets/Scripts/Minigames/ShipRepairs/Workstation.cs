using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Workstation : MonoBehaviour
{
    public GameObject greenZone;
    public GameObject indicator;

    public float rotationSpeed;

    public bool isRotating; 

    // Move all fields below to the manager/constants class 
    public bool minigameStarted; 

    private int points;
    private int consecutiveWins;

    // Hide these later 
    private string pointsText;
    private string feedbackText;
    private string successMessage = "Success!";
    private string failureMessage = "Failure!";
    private string midRotationMessage = "Spinning...";

    private void Start()
    {
        rotationSpeed = 8f; 
    }

    public void StartRotating()
    {
        isRotating = true;
        feedbackText = midRotationMessage;
    }

    public void StopRotating()
    {
        isRotating = false;
    }

    public void PlayerWins()
    {
        Debug.Log(successMessage);
        feedbackText = successMessage;
        points++;
        consecutiveWins++;
    }

    public void PlayerLoses()
    {
        Debug.Log(failureMessage);
        feedbackText = failureMessage;
        consecutiveWins = 0; 
    }

    private void StartMinigame()
    {
        minigameStarted = true;
        isRotating = true;
    }

    public void RotateWorkStation()
    {
        transform.eulerAngles += new Vector3(0f, 0f, rotationSpeed);
    }

    // This will increase the difficulty by disorientating the player 
    public void ReverseRotationDirection()
    {
        rotationSpeed = rotationSpeed > 0 ? rotationSpeed * -1 : Mathf.Abs(rotationSpeed); 
    }

    // This will increase the difficulty by decreasing the timing window 
    public void IncreaseRotationSpeed(float speedIncrease) 
    {
        rotationSpeed += speedIncrease;
    }

    private void Update()
    {
        // The branching of this block could definitely be improved. 
        if (Input.GetMouseButtonUp(1))
        {
            // If we don't check for this, 
            // the player will win when starting the first spin.
            if (!minigameStarted)
            {
                StartMinigame(); 
            }
            else
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
        }

        if (isRotating)
        {
            RotateWorkStation();
        }
    }

    private void OnGUI()
    {
        
    }
}
