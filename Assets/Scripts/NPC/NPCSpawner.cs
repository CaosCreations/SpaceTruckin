using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnDateTime> spawnDateTimes;

    [SerializeField] private GameObject npcObject;

    private void Start()
    {
        CalendarManager.OnEndOfDay += CueSpawnsForToday;
        
        // If we have passed any spawn dates set in the inspector when the game starts,
        // then remove them from the List.
        RemovePastSpawnDateTimes();
    }

    private void OnValidate()
    {
        ValidateFields();
    }

    private void CueSpawnsForToday()
    {
        // Get spawns scheduled for today's date (there can be multiple) 
        var spawnsForToday = spawnDateTimes
            .Where(x => CalendarManager.DateIsToday(x.SpawnDate))
            .ToList();

        // Get despawns scheduled for today's date (there can be multiple)
        var despawnsForToday = spawnDateTimes
            .Where(x => CalendarManager.DateIsToday(x.DespawnDate))
            .ToList();

        // Start coroutines that will wait until a certain time and then either spawn or despawn
        spawnsForToday.ForEach(x => SpawnAtTime(x.SpawnTime, isActive: true));

        despawnsForToday.ForEach(x => SpawnAtTime(x.DespawnTime, isActive: false));
    }

    /// <summary>
    /// Spawn/despawn an NPC at a specified time of day
    /// </summary>
    /// <param name="timeOfDay">The time to spawn/despawn at</param>
    /// <param name="isActive">Whether the NPC is set to active or inactive (spawn/despawn)</param>
    private void SpawnAtTime(TimeOfDay timeOfDay, bool isActive)
    {
        // The difference in game-time seconds between the start of the day and the scheduled spawn time.
        double secondsFromDayStart = (timeOfDay.ToTimeSpan() - CalendarManager.Instance.DayStartTime).TotalSeconds;

        // Convert time period to real-time seconds that can be passed to a Coroutine. 
        float realTimeSecondsToWait = secondsFromDayStart.ToRealTimeSeconds();

        StartCoroutine(SpawnAfterDelay(realTimeSecondsToWait, isActive));
    }

    private IEnumerator SpawnAfterDelay(float secondsToWait, bool isActive)
    {
        yield return new WaitForSeconds(secondsToWait);
        npcObject.SetActive(isActive);
    }

    private void RemovePastSpawnDateTimes()
    {
        spawnDateTimes.ForEach(x =>
        {
            if (x.SpawnDate < CalendarManager.Instance.CurrentDate
                && x.DespawnDate < CalendarManager.Instance.CurrentDate)
            {
                spawnDateTimes.Remove(x);
            }
        });
    }

    private void ValidateFields()
    {
        foreach (SpawnDateTime spawnDateTime in spawnDateTimes)
        {
            // Validate spawn dates/times
            spawnDateTime.SpawnDate = spawnDateTime.SpawnDate.Validate();
            spawnDateTime.SpawnTime = spawnDateTime.SpawnTime.Validate();

            // Validate despawn dates/times 
            spawnDateTime.DespawnDate = spawnDateTime.DespawnDate.Validate();
            spawnDateTime.DespawnTime = spawnDateTime.DespawnTime.Validate();
        }
    }
}
