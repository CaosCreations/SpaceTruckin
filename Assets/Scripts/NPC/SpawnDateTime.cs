using System;

/// <summary>
/// Each NPC spawn has a time at which they will spawn 
/// and a time at which they will despawn again.
/// </summary>
[Serializable]
public class SpawnDateTime
{
    public Date SpawnDate;
    public TimeOfDay SpawnTime;

    public Date DespawnDate;
    public TimeOfDay DespawnTime;
}
