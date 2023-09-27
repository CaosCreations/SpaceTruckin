using Events;
using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;

public class ClockManager : MonoBehaviour, ILuaFunctionRegistrar
{
    public static TimeSpan CurrentTime { get; private set; }
    public static TimeOfDay.Phase CurrentTimeOfDayPhase => isEvening ? TimeOfDay.Phase.Evening : TimeOfDay.Phase.Morning;

    public static int TickSpeedMultiplier { get; private set; }
    private static int currentTimeInSeconds;

    private static string dateTimeText;

    private static bool clockStopped;
    private static bool isEvening;

    [SerializeField]
    private Cutscene clockStartCutscene;

    [SerializeField]
    private bool showOnGui = false;

    private void Start()
    {
        CalculateTickSpeedMultiplier();
        RegisterEvents();

        if (!CalendarManager.IsTimeFrozenToday)
        {
            StartClock();
        }
        else
        {
            StopClock();
        }
        UpdateDateTimeText();
#if UNITY_EDITOR
        Application.targetFrameRate = PlayerConstants.EditorTargetFrameRate;
#endif
    }

    private void RegisterEvents()
    {
        SingletonManager.EventService.Add<OnPlayerSleepEvent>(OnPlayerSleepHandler);
        SingletonManager.EventService.Add<OnPlayerPausedEvent>(OnPlayerPausedHandler);
        SingletonManager.EventService.Add<OnPlayerUnpausedEvent>(OnPlayerUnpausedHandler);
        SingletonManager.EventService.Add<OnUITransitionEndedEvent>(OnUITransitionEndedHandler);
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
    }

    // Calculate how quick the clock should tick relative to real time 
    private void CalculateTickSpeedMultiplier()
    {
        TickSpeedMultiplier = Convert.ToInt32(CalendarManager.AwakeTimeDuration.TotalSeconds) / CalendarManager.RealTimeDayDurationInSeconds;
    }

    private static void ResetClock()
    {
        clockStopped = true;
        CurrentTime = CalendarManager.DayStartTime;
        currentTimeInSeconds = (int)CurrentTime.TotalSeconds;
    }

    public static void StartClock()
    {
        clockStopped = false;
    }

    public static void StopClock()
    {
        clockStopped = true;
    }

    /// <summary>
    /// The day either ends when the player chooses to sleep or the time elapses.
    /// </summary>
    private void EndDay()
    {
        // Notify other objects that the day has ended
        SingletonManager.EventService.Dispatch(new OnEndOfDayClockEvent());

        isEvening = false;
        ResetClock();
    }

    private void OnPlayerSleepHandler(OnPlayerSleepEvent evt)
    {
        EndDay();
    }

    public static void SetCurrentTime(int seconds, bool overrideTransition = false)
    {
        if (overrideTransition && !clockStopped)
        {
            clockStopped = true;
        }

        currentTimeInSeconds = seconds;
        CurrentTime = TimeSpan.FromSeconds(currentTimeInSeconds);

        if (overrideTransition)
        {
            isEvening = CurrentTime >= CalendarManager.EveningStartTime;
            clockStopped = false;
        }
    }

    private void Update()
    {
        if (!clockStopped)
        {
            currentTimeInSeconds += Convert.ToInt32(Time.deltaTime * TickSpeedMultiplier);
            CurrentTime = TimeSpan.FromSeconds(currentTimeInSeconds);

            UpdateDateTimeText();
        }

        // Go to next day after day end time 
        if (CurrentTime >= CalendarManager.DayEndTime)
        {
            EndDay();
        }

        // Night lighting time 
        if (LightingManager.CurrentState != LightingState.Night && CurrentTime >= LightingManager.LightsOutTime)
        {
            SingletonManager.EventService.Dispatch<OnLightsOutTimeEvent>();
        }

        // Evening time 
        if (!isEvening && CurrentTime >= CalendarManager.EveningStartTime)
        {
            StartEvening();
        }
    }

    private void StartEvening()
    {
        isEvening = true;
        SingletonManager.EventService.Dispatch<OnEveningStartEvent>();
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

    private void OnPlayerPausedHandler(OnPlayerPausedEvent evt)
    {
        if (evt.StopClock)
            StopClock();
    }

    private void OnPlayerUnpausedHandler()
    {
        if (CalendarManager.IsTimeFrozenToday)
            return;

        StartClock();
    }

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        if (evt.Cutscene == clockStartCutscene)
        {
            StartClock();
        }
    }

    private void OnUITransitionEndedHandler(OnUITransitionEndedEvent evt)
    {
        if (evt.TransitionType == TransitionUI.TransitionType.TimeOfDay && !isEvening)
        {
            if (!CalendarManager.IsTimeFrozenToday)
            {
                StartClock();
            }
            else
            {
                StopClock();
            }
            SingletonManager.EventService.Dispatch<OnMorningStartEvent>();
        }
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

    private void PassMinutes(double minutes)
    {
        Debug.Log($"Adding {minutes} minutes to current time");
        currentTimeInSeconds += (int)Math.Floor(minutes * 60);
    }

    public void RegisterLuaFunctions()
    {
        Lua.RegisterFunction(DialogueConstants.PassMinutesFunctionName, this, SymbolExtensions.GetMethodInfo(() => PassMinutes(0D)));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.PassMinutesFunctionName);
    }
}
