using System;
using UnityEngine;

[Serializable]
public class NPCLocationByDateContainer : ContainerWithLookup<Date, NPCLocation>
{
    [field: SerializeField]
    public NPCLocationByDate[] LocationsByDate { get; private set; }

    public override void InitLookup()
    {
        Lookup = new();

        foreach (var locationByDate in LocationsByDate)
        {
            if (!Lookup.TryAdd(locationByDate.Date, locationByDate.Location))
            {
                Debug.LogError("Error adding location by date to item lookup. Key: " + locationByDate.Date);
            }
        }
    }
}