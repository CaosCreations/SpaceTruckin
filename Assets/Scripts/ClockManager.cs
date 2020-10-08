using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public static TimeSpan currentTime; 
    private TimeSpan dayStartTime = new TimeSpan(6, 0, 0); // 6am
    private TimeSpan dayEndTime = new TimeSpan(26, 0, 0); // 2am the next day

    private int currentTimeInSeconds;
    private int tickSpeed = 80; 

    private bool clockStopped; 

    public Text clockText;

    private void Start()
    {
        ResetClock(); 
        StartClock();
    }

	private void StartClock()
	{
        clockStopped = false; 
		StartCoroutine(TickClock());
	}

	private IEnumerator TickClock()
	{
        //if (clockStopped)
        //{
        //    yield break; 
        //}

		while (!clockStopped)
		{
			currentTimeInSeconds += tickSpeed;
            currentTime = TimeSpan.FromSeconds(currentTimeInSeconds);
            //clockText.text = currentTime.ToString("hh':'mm");

            LogClockData();

			yield return new WaitForSeconds(1);
		}
	}

	private void StopClock()
    {
        clockStopped = true;
    }

    private void ResetClock()
    {
        currentTime = dayStartTime;
        currentTimeInSeconds = (int)currentTime.TotalSeconds;
        //clockText.text = currentTime.ToString();
    }

    private void LogClockData()
    {
        Debug.Log("Current time: " + currentTime);
        Debug.Log("Current time in seconds: " + currentTimeInSeconds);
        Debug.Log("Time remaining: " + dayEndTime.Subtract(currentTime)); 
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            StopClock();
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            StartClock(); 
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(124f, 124f, 256f, 256f), currentTime.ToString("hh':'mm")); 
    }
}

