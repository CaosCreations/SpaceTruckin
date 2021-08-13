using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LightingData", menuName = "ScriptableObjects/LightingData", order = 1)]
public class LightingData : ScriptableObject
{
    [SerializeField] private TimeOfDay lightsOutTime;

    public TimeSpan LightsOutTime;

    [field: SerializeField] public float DayTimeIntensity { get; private set; }
    [field: SerializeField] public float NightTimeIntensity { get; private set; }

    private void OnValidate()
    {
        LightsOutTime = lightsOutTime.ToTimeSpan();
    }
}
