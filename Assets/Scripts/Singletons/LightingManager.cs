using System;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public static LightingManager Instance { get; private set; }

    [SerializeField] private LightingData lightingData;

    #region Property Accessors 
    public static TimeSpan LightsOutTime => Instance.lightingData.LightsOutTime;
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

    private static void SetLightIntensity(Light light, float intensity)
    {
        light.intensity = intensity;
    }

    public static void TurnOnTheLights()
    {
        SetLightIntensity(Instance.lightingData.StationLight, Instance.lightingData.DayTimeIntensity);
    }

    public static void DimTheLights()
    {
        SetLightIntensity(Instance.lightingData.StationLight, Instance.lightingData.NightTimeIntensity);
    }
}
