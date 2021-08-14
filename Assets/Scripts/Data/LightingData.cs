using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LightingData", menuName = "ScriptableObjects/LightingData", order = 1)]
public class LightingData : ScriptableObject
{
    [SerializeField] private TimeOfDay lightsOutTime;

    public TimeSpan LightsOutTime;

    [field: SerializeField] public float DayTimeIntensity { get; private set; }
    [field: SerializeField] public float NightTimeIntensity { get; private set; }
    [field: SerializeField] public float LightChangeDurationInSeconds { get; private set; }
    [field: SerializeField] public float LightChangeTickCount { get; private set; }

    private void OnValidate()
    {
        // Convert to a TimeSpan that can be used by the ClockManager
        LightsOutTime = lightsOutTime.ToTimeSpan();
    }
}
