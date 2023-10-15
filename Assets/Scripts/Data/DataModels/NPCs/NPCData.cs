using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "ScriptableObjects/NPCData", order = 1)]
public class NPCData : ScriptableObject
{
    [field: Header("Location")]
    [field: Tooltip("Where the NPC appears by default in the morning and evening respectively")]
    [field: SerializeField]
    public NPCLocation DefaultLocation { get; private set; } = new();

    [field: Tooltip("Where the NPC appears in the morning and evening on specific dates")]
    [field: SerializeField]
    public NPCLocationByDateContainer LocationByDateContainer { get; private set; } = new();

    [field: Header("Animation")]
    [field: Tooltip("What animation parameters are active by default in the morning and evening respectively")]
    [field: SerializeField]
    public NPCAnimationContext DefaultAnimationContext { get; private set; } = new();

    [field: Tooltip("What animation parameters are active in the morning and evening on specific dates")]
    [field: SerializeField]
    public NPCAnimationContextByDateContainer AnimationContextByDateContainer { get; private set; } = new();

    public NPCLocation GetLocationByDate(Date date)
    {
        // If the location needs to be overridden for this date, then return the one from the date map
        // Having generic Conditions is janky when we're already indexed by Date, but leave here for now for backwards compatibility
        if (!LocationByDateContainer.Lookup.TryGetValue(date, out var value) || !value.Conditions.AreAllMet())
        {
            return DefaultLocation;
        }
        return value;
    }

    public NPCAnimationContext GetAnimationContextByDate(Date date)
    {
        // If the location needs to be overridden for this date, then return the one from the date map
        if (AnimationContextByDateContainer.Lookup.TryGetValue(date, out var value))
        {
            return value;
        }
        return DefaultAnimationContext;
    }
}