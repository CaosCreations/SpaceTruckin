using System;
using UnityEngine;

/// <summary>
/// Represents a time of day in a format that can be set in the inspector.
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

    public static int TickSpeedMultiplier { get; private set; }
    private static int currentTimeInSeconds;

    private static string dateTimeText;

    private static bool clockStopped;

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
        TickSpeedMultiplier = Convert.ToInt32(
            CalendarManager.Instance.AwakeTimeDuration.TotalSeconds)
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
            currentTimeInSeconds += Convert.ToInt32(Time.deltaTime * TickSpeedMultiplier);
            CurrentTime = TimeSpan.FromSeconds(currentTimeInSeconds);

            UpdateDateTimeText();
        }
    }

    private static void UpdateDateTimeText()
    {
        dateTimeText = $"{CurrentTime:hh':'mm}\n{CalendarManager.Instance.CurrentDate}";
    }

    private void OnGUI()
    {
        var localStyle = new GUIStyle();
        localStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(
            Camera.main.pixelWidth - 128f, Camera.main.pixelHeight - 128f, 128f, 128f),
            dateTimeText);
    }

    private void LogClockData()
    {
        Debug.Log("Current time: " + CurrentTime);
        Debug.Log("Current time in seconds: " + currentTimeInSeconds);
        Debug.Log("Time remaining: " + CalendarManager.Instance.DayEndTime.Subtract(CurrentTime));
    }
}
