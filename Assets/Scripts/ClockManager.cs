using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public static TimeSpan currentTime; 

    // This doesn't exist yet 
    public Text clockText;

    // Hide these later 
    private TimeSpan dayStartTime = new TimeSpan(6, 0, 0); // 6am
    private TimeSpan dayEndTime = new TimeSpan(26, 0, 0); // 2am the next day

    [SerializeField]
    private int realTimeDayDurationInSeconds = 900; // 15 mins  

    private int currentTimeInSeconds;
    private int tickSpeedMultiplier;

    private bool clockStopped; 

    private void Start()
    {
		// Temporary
        CanvasManager.onCanvasActivated += StopClock;
        CanvasManager.onCanvasDeactivated += StartClock; 

        CalculateTickSpeedMultiplier();
        StartDay();
    }

    // Calculate how quick the clock should tick relative to real time 
    private void CalculateTickSpeedMultiplier()
    {
        tickSpeedMultiplier = Convert.ToInt32(dayEndTime.Subtract(dayStartTime).TotalSeconds)
            / realTimeDayDurationInSeconds;
    }

    private void StartDay()
    {
        ResetClock();
        StartClock(); 
        // Other startup logic tbd 
    }

	private void StartClock()
	{
        clockStopped = false; 
	}
    private void ResetClock()
    {
        clockStopped = false;
        currentTime = dayStartTime;
        currentTimeInSeconds = (int)currentTime.TotalSeconds;
        //clockText.text = currentTime.ToString();
    }

	private void StopClock()
    {
        clockStopped = true;
    }

    private void EndDay()
    {
        ResetClock(); 
        // Other ending logic tbd 
        // Game will force player to sleep if not already 
    }

    private void Update()
    {
        if (currentTime >= dayEndTime)
        {
            EndDay();
        }

        if (!clockStopped)
        {
            currentTimeInSeconds += Convert.ToInt32(Time.deltaTime * tickSpeedMultiplier);
            currentTime = TimeSpan.FromSeconds(currentTimeInSeconds);
            //clockText.text = currentTime.ToString("hh':'mm");
        }
    }

    private void OnGUI()
    {
        var localStyle = new GUIStyle();
        localStyle.normal.textColor = Color.blue;

        GUI.Label(new Rect(
            Camera.main.pixelWidth - 128f, Camera.main.pixelHeight - 128f, 128f, 128f), 
            currentTime.ToString("hh':'mm"), localStyle);

    }

    private void LogClockData()
    {
        Debug.Log("Current time: " + currentTime);
        Debug.Log("Current time in seconds: " + currentTimeInSeconds);
        Debug.Log("Time remaining: " + dayEndTime.Subtract(currentTime));
    }
}
