using Events;
using System;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public static TimeSpan CurrentTime;

    public static int TickSpeedMultiplier { get; private set; }
    private static int currentTimeInSeconds;

    private static string dateTimeText;

    private static bool clockStopped;
    private static bool isEvening;
    private static bool showOnGui = false;

    private void Start()
    {
        UIManager.OnCanvasActivated += StopClock;
        UIManager.OnCanvasDeactivated += StartClock;

        CalculateTickSpeedMultiplier();
        SetupClockForNextDay();

        SingletonManager.EventService.Add<OnPlayerSleepEvent>(OnPlayerSleepHandler);

#if UNITY_EDITOR
        Application.targetFrameRate = PlayerConstants.EditorTargetFrameRate;
#endif
    }

    // Calculate how quick the clock should tick relative to real time 
    private void CalculateTickSpeedMultiplier()
    {
        TickSpeedMultiplier = Convert.ToInt32(
            CalendarManager.AwakeTimeDuration.TotalSeconds) / CalendarManager.RealTimeDayDurationInSeconds;
    }

    public void SetupClockForNextDay()
    {
        ResetClock();
        StartClock();
    }

    public void ResetClock()
    {
        clockStopped = true;
        CurrentTime = CalendarManager.DayStartTime;
        currentTimeInSeconds = (int)CurrentTime.TotalSeconds;
    }

    public void StartClock()
    {
        clockStopped = false;
        SingletonManager.EventService.Dispatch(new OnClockStartedEvent());
    }

    private void StopClock()
    {
        clockStopped = true;
        SingletonManager.EventService.Dispatch(new OnClockStoppedEvent());
    }

    /// <summary>
    /// The day either ends when the player chooses to sleep or the time elapses.
    /// </summary>
    private void EndDay()
    {
        // Notify other objects that the day has ended
        SingletonManager.EventService.Dispatch(new OnEndOfDayEvent());

        SetupClockForNextDay();
    }

    private void OnPlayerSleepHandler(OnPlayerSleepEvent evt)
    {
        EndDay();
    }

    private void Update()
    {
        if (!clockStopped)
        {
            currentTimeInSeconds += Convert.ToInt32(Time.deltaTime * TickSpeedMultiplier);
            CurrentTime = TimeSpan.FromSeconds(currentTimeInSeconds);

            UpdateDateTimeText();
        }

        if (CurrentTime >= CalendarManager.DayEndTime)
        {
            EndDay();
        }

        if (CurrentTime >= LightingManager.LightsOutTime && LightingManager.CurrentState != LightingState.Night)
        {
            LightingManager.ChangeInternalLighting(LightingState.Night);
        }
    }

    private static void UpdateDateTimeText()
    {
        dateTimeText = GetDateTimeText();
    }

    public static string GetDateTimeText()
    {
        var dateTimeText = $"{CurrentTime:hh':'mm}\n{CalendarManager.CurrentDate}";
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
        Debug.Log("Time remaining: " + CalendarManager.DayEndTime.Subtract(CurrentTime));
    }
}
