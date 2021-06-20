using System;
using UnityEngine;

/// <summary>
/// Represent a time of day in a format that can be set in the inspector.
/// </summary>
[Serializable]
public struct TimeOfDay
{
    public int Hours;
    public int Minutes;
    public int Seconds;
}

public class ClockManager : MonoBehaviour
{
    public static TimeSpan CurrentTime;

    private int currentTimeInSeconds;
    private int tickSpeedMultiplier;

    private bool clockStopped;

    private void Start()
    {
        UIManager.OnCanvasActivated += StopClock;
        UIManager.OnCanvasDeactivated += StartClock;

        CalculateTickSpeedMultiplier();
        SetupClockForNextDay();

#if UNITY_EDITOR
        Application.targetFrameRate = PlayerConstants.EditorTargetFramerate;
#endif
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
        CurrentTime = CalendarManager.Instance.DayStartTime;
        currentTimeInSeconds = (int)CurrentTime.TotalSeconds;
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
        if (CurrentTime >= CalendarManager.Instance.DayEndTime)
        {
            CalendarManager.EndDay();
        }

        if (!clockStopped)
        {
            currentTimeInSeconds += Convert.ToInt32(Time.deltaTime * tickSpeedMultiplier);
            CurrentTime = TimeSpan.FromSeconds(currentTimeInSeconds);
        }
    }

    private void OnGUI()
    {
        var localStyle = new GUIStyle();
        localStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(
            Camera.main.pixelWidth - 128f, Camera.main.pixelHeight - 128f, 128f, 128f),
            CurrentTime.ToString("hh':'mm"), localStyle);

    }

    private void LogClockData()
    {
        Debug.Log("Current time: " + CurrentTime);
        Debug.Log("Current time in seconds: " + currentTimeInSeconds);
        Debug.Log("Time remaining: " + CalendarManager.Instance.DayEndTime.Subtract(CurrentTime));
    }
}
