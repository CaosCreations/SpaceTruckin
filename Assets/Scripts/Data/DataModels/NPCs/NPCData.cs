﻿using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "ScriptableObjects/NPCData", order = 1)]
public class NPCData : ScriptableObject
{
    [Tooltip("Where the NPC appears by default in the morning and evening respectively")]
    [field: SerializeField]
    public NPCLocation DefaultLocation { get; private set; } = new();

    [Tooltip("Where the NPC appears in the morning and evening on specific dates")]
    [field: SerializeField]
    public NPCLocationByDateContainer LocationByDateContainer { get; private set; } = new();

    public NPCLocation GetLocationByDate(Date date)
    {
        NPCLocation location = DefaultLocation;

        // If the location needs to be overridden for this date, then return the one from the date map
        if (LocationByDateContainer.Lookup.TryGetValue(date, out var value))
        {
            location = value;
        }
        return location;
    }
}