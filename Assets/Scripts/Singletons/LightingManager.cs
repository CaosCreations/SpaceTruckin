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
        float timePerTick = secondsToWait / 100;
        float intensityPerTick = targetIntensity / (secondsToWait + 1) / 100;

        if (targetIntensity < light.intensity)
            intensityPerTick *= -1;

        Instance.StartCoroutine(WaitForLights(light, targetIntensity, intensityPerTick, timePerTick));
    }

    private static IEnumerator WaitForLights(Light light, float targetIntensity, float intensityPerTick, float timePerTick)
    {
        while (light.intensity != targetIntensity)
        {
            light.intensity += intensityPerTick;
            yield return new WaitForSeconds(timePerTick);
        }
    }

    public static void TurnOnTheLights()
    {
        Array.ForEach(Instance.lightsToControl, 
            (x) => SetLightIntensity(x, DayTimeIntensity, LightChangeDurationInSeconds));
    }

    public static void DimTheLights()
    {
        Array.ForEach(Instance.lightsToControl, 
            (x) => SetLightIntensity(x, NightTimeIntensity, LightChangeDurationInSeconds));
    }
}
