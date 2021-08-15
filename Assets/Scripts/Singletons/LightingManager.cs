using System;
using System.Collections;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public static LightingManager Instance { get; private set; }

    [SerializeField] private LightingData lightingData;

    [SerializeField] private Light[] lightsToControl;

    #region Property Accessors 
    public static TimeSpan LightsOutTime => Instance.lightingData.LightsOutTime;
    private static float LightChangeDurationInSeconds => Instance.lightingData.LightChangeDurationInSeconds;
    private static float DayTimeIntensity => Instance.lightingData.DayTimeIntensity;
    private static float NightTimeIntensity => Instance.lightingData.NightTimeIntensity;
    private static float LightChangeTickCount => Instance.lightingData.LightChangeTickCount;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CalendarManager.OnEndOfDay += TurnOnTheLights;
    }

    private static void SetLightIntensity(Light light, float targetIntensity, float secondsToWait = 0)
    {
        Instance.StartCoroutine(WaitForIntensityToChange(light, targetIntensity, secondsToWait));
    }

    private static IEnumerator WaitForIntensityToChange(Light light, float targetIntensity, float secondsToWait)
    {
        float timePerTick = (float)Math.Max(0.0001D, secondsToWait) / LightChangeTickCount;

        float intensityPerTick = (targetIntensity - light.intensity) / LightChangeTickCount;

        float counter = LightChangeTickCount;

        while (counter > 0)
        {
            light.intensity += intensityPerTick;
            counter--;

            yield return new WaitForSeconds(timePerTick);
        }
    }

    public static void TurnOnTheLights()
    {
        Array.ForEach(Instance.lightsToControl,
            (x) => SetLightIntensity(x, DayTimeIntensity));
    }

    public static void TurnOffTheLights()
    {
        Array.ForEach(Instance.lightsToControl,
            (x) => SetLightIntensity(x, NightTimeIntensity, LightChangeDurationInSeconds));
    }
}
