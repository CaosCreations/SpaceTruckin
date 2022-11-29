using System;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public static TimeSpan CurrentTime;

    public static int TickSpeedMultiplier { get; private set; }
    private static int currentTimeInSeconds;

    private static string dateTimeText;

    private static bool clockStopped;
    private static bool showOnGui = false;

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

        if (CurrentTime == LightingManager.LightsOutTime)
        {
            LightingManager.ChangeInternalLighting(LightingState.Night);
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
        dateTimeText = GetDateTimeText();
    }

    public static string GetDateTimeText()
    {
        var dateTimeText = $"{CurrentTime:hh':'mm}\n{CalendarManager.Instance.CurrentDate}";
        return dateTimeText;
    }

    private void OnGUI()
    {
        if (!showOnGui)
            return;

        var localStyle = new GUIStyle(GUI.skin.box);
        localStyle.normal.textColor = Color.white;
        localStyle.font = FontManager.Instance.GetFontByType(FontType.Subtitle);
        localStyle.fontSize = 18;

        GUI.backgroundColor = Color.black;

        GUI.Label(new Rect(
            UIConstants.ClockTextXPosition,
            UIConstants.ClockTextYPosition,
            UIConstants.ClockTextWidth,
            UIConstants.ClockTextHeight),
            dateTimeText,
            localStyle);
    }

    private void LogClockData()
    {
        Debug.Log("Current time: " + CurrentTime);
        Debug.Log("Current time in seconds: " + currentTimeInSeconds);
        Debug.Log("Time remaining: " + CalendarManager.Instance.DayEndTime.Subtract(CurrentTime));
    }
}
