using System;
using UnityEngine;

/// <summary>
/// Mapping between animation params and specific <see cref="Date"/>s.
/// </summary>
[Serializable]
public class NPCAnimationContextByDateContainer : ContainerWithLookup<Date, NPCAnimationContext>
{
    [field: SerializeField]
    public NPCAnimationContextByDate[] AnimationContextsByDate { get; private set; }

    public override bool InitLookup()
    {
        var success = true;
        Lookup = new();

        foreach (var animationContextByDate in AnimationContextsByDate)
        {
            if (!Lookup.TryAdd(animationContextByDate.Date, animationContextByDate.AnimationContext))
            {
                Debug.LogError("Error adding animation context by date to item lookup. Key: " + animationContextByDate.Date);
                success = false;
            }
        }
        return success;
    }
}