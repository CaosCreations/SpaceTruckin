﻿using System;
using UnityEngine;

public enum LightingState
{
    Day, Night
}

[CreateAssetMenu(fileName = "LightingData", menuName = "ScriptableObjects/LightingData", order = 1)]
public class LightingData : ScriptableObject
{
    [SerializeField] private TimeOfDay lightsOutTime;

    public TimeSpan LightsOutTime;

    #region Light Change Properties
    [field: SerializeField] public float InternalDayLightsIntensity { get; private set; }
    [field: SerializeField] public float InternalNightLightsIntensity { get; private set; }
    [field: SerializeField] public float ExternalLightMinimumIntensity { get; private set; }
    [field: SerializeField] public float ExternalLightMaximumIntensity { get; private set; }
    [field: SerializeField] public float ExternalLightChangeDurationInSeconds { get; private set; }
    [field: SerializeField] public float ExternalLightChangeTickCount { get; private set; }
    #endregion

    private void OnValidate()
    {
        // Convert to a TimeSpan that can be used by the ClockManager
        LightsOutTime = lightsOutTime.ToTimeSpan();
    }
}