using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnDateTime> spawnDateTimes;

    [SerializeField] private GameObject npcObject;

    private bool IsSpawned => npcObject.activeSelf;

    private void Start()
    {
        CalendarManager.OnEndOfDay += CueSpawnsForToday;
        RemovePastSpawnDateTimes();
    }

    private void OnValidate()
    {
        ValidateFields();
    }

    private void CueSpawnsForToday()
    {
        // Get spawns scheduled for today's date 
        var spawnsForToday = spawnDateTimes
            .Where(x => CalendarManager.DateIsToday(x.SpawnDate))
            .ToList();
        
        var despawnsForToday = spawnDateTimes
            .Where(x => CalendarManager.DateIsToday(x.DespawnDate))
            .ToList();

        Debug.Log(ClockManager.CurrentTime);

        // Start coroutines that will wait until a certain time and then act
        spawnsForToday.ForEach(x => SpawnAtTime(x.SpawnTime));

        despawnsForToday.ForEach(x => DespawnAtTime(x.DespawnTime));
    }

    private void SpawnAtTime(TimeOfDay timeOfDay)
    {
        //// Subtract the spawn's time of day from the time that the day begins 
        //int realTimeSecondsToWait = (int)(timeOfDay
        //    .ToRealTimeSeconds() - CalendarManager.Instance.DayStartTime.TotalSeconds);

        int realTimeSecondsToWait = timeOfDay
            .ToRealTimeSeconds() - CalendarManager.Instance.DayStartTime
            .ToTimeOfDay()
            .ToRealTimeSeconds();

        // Todo: start coroutines at 0:00 for the current day. 
        // Ignore the day start time. 

        StartCoroutine(WaitUntilSpawnTime(realTimeSecondsToWait));
        
        npcObject.SetActive(true);
    }

    private void DespawnAtTime(TimeOfDay timeOfDay)
    {
        StartCoroutine(WaitUntilSpawnTime(timeOfDay.ToSeconds()));
        npcObject.SetActive(false);
    }

    private IEnumerator WaitUntilSpawnTime(int secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
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
            spawnDateTime.SpawnDate = spawnDateTime.SpawnDate.Validate();
            spawnDateTime.SpawnTime = spawnDateTime.SpawnTime.Validate();

            spawnDateTime.DespawnDate = spawnDateTime.DespawnDate.Validate();
            spawnDateTime.DespawnTime = spawnDateTime.DespawnTime.Validate();
        }
    }
}
