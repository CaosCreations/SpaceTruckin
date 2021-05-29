using System;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public static TimeSpan currentTime; 

    // This will replace OnGUI when the design is ready 
    public Text clockText; 

    private int currentTimeInSeconds;
    private int tickSpeedMultiplier;

    private bool clockStopped; 

    private void Start()
    {
        UIManager.OnCanvasActivated += StopClock;
        UIManager.OnCanvasDeactivated += StartClock;

        CalculateTickSpeedMultiplier();
        SetupClockForNextDay();
    }

    // Calculate how quick the clock should tick relative to real time 
    private void CalculateTickSpeedMultiplier()
    {
        tickSpeedMultiplier = Convert.ToInt32(
            CalendarManager.Instance.DayEndTime.Subtract(CalendarManager.Instance.DayStartTime).TotalSeconds)
                / CalendarManager.Instance.RealTimeDayDurationInSeconds;
    }

    public void SetupClockForNextDay()
    {
        ResetClock();
        StartClock(); 
    }
	
	public void ResetClock()
    {
        clockStopped = true;
        currentTime = CalendarManager.Instance.DayStartTime;
        currentTimeInSeconds = (int)currentTime.TotalSeconds;
        //clockText.text = currentTime.ToString();
    }

    public void StartClock()
    {
        clockStopped = false;
    }

    private void StopClock()
    {
        clockStopped = true;
    }

    private void Update()
    {
        if (currentTime >= CalendarManager.Instance.DayEndTime)
        {
            CalendarManager.EndDay();
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
        Debug.Log("Time remaining: " + CalendarManager.Instance.DayEndTime.Subtract(currentTime));
    }
}
