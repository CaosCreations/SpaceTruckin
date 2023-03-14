using Events;
using System;
using System.Collections;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public static LightingManager Instance { get; private set; }

    [SerializeField] private LightingData lightingData;

    /// <summary> Artifical lights that are on in the daytime </summary>
    [SerializeField] private Light[] internalDayLights;

    /// <summary> Artifical lights that are on during the night </summary>
    [SerializeField] private Light[] internalNightLights;

    /// <summary> Natural light outside of the station. There is only one. </summary>
    [SerializeField] private Light[] externalLights;

    private LightingState currentState = LightingState.Day;

    #region Property Accessors 
    public static TimeSpan LightsOutTime => Instance.lightingData.LightsOutTime;
    public static LightingState CurrentState => Instance.currentState;
    private static float LightChangeDurationInSeconds => Instance.lightingData.ExternalLightChangeDurationInSeconds;
    private static float InternalDayLightsIntensity => Instance.lightingData.InternalDayLightsIntensity;
    private static float InternalNightLightsIntensity => Instance.lightingData.InternalNightLightsIntensity;
    private static float ExternalLightsMinimumIntensity => Instance.lightingData.ExternalLightMinimumIntensity;
    private static float ExternalLightsMaximumIntensity => Instance.lightingData.ExternalLightMaximumIntensity;
    private static float ExternalLightChangeTickCount => Instance.lightingData.ExternalLightChangeTickCount;
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
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
    }

    private static void SetLightIntensity(Light light, float targetIntensity, float secondsToWait = 0, float numberOfTicks = 1)
    {
        if (secondsToWait > 0)
        {
            // Handle gradual transitions 
            Instance.StartCoroutine(WaitForIntensityToChange(light, targetIntensity, secondsToWait, numberOfTicks));
        }
        else
        {
            // Handle static switching 
            light.intensity = targetIntensity;
        }
    }

    private static IEnumerator WaitForIntensityToChange(Light light, float targetIntensity, float secondsToWait, float numberOfTicks)
    {
        float timePerTick = (float)Math.Max(0.0001D, secondsToWait) / (float)Math.Max(0.0001D, numberOfTicks);

        float intensityPerTick = (targetIntensity - light.intensity) / numberOfTicks;

        float counter = numberOfTicks;

        while (counter > 0)
        {
            light.intensity += intensityPerTick;
            counter--;

            // On very low frame rates, we may have to use FixedUpdate or InvokeRepeating
            yield return new WaitForSecondsRealtime(timePerTick);
        }
    }

    public static void ChangeInternalLighting(LightingState lightingState)
    {
        if (lightingState == CurrentState)
        {
            Debug.Log("Lighting state arg has same value as current lighting state. Won't change lighting.");
            return;
        }

        switch (lightingState)
        {
            case LightingState.Day:
                Instance.internalDayLights.SetIntensities(InternalDayLightsIntensity);
                Instance.internalNightLights.SetIntensities(0);
                break;
            case LightingState.Night:
                Instance.internalDayLights.SetIntensities(0);
                Instance.internalNightLights.SetIntensities(InternalNightLightsIntensity);
                break;
        }

        Instance.currentState = lightingState;
    }

    public static void TransitionExternalLightsOn()
    {
        Array.ForEach(Instance.externalLights,
            (x) => SetLightIntensity(x, InternalDayLightsIntensity));
    }

    public static void TransitionExternalLightsOff()
    {
        Array.ForEach(Instance.externalLights,
            (x) => SetLightIntensity(x, InternalNightLightsIntensity, LightChangeDurationInSeconds));
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        ChangeInternalLighting(LightingState.Day);
    }
}
