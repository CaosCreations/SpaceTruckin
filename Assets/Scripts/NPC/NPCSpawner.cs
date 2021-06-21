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

        // Start coroutines that will wait until a certain time and then act
        spawnsForToday.ForEach(x => SpawnAtTime(x.SpawnTime));

        despawnsForToday.ForEach(x => DespawnAtTime(x.DespawnTime));
    }

    private void SpawnAtTime(TimeOfDay timeOfDay)
    {
        StartCoroutine(WaitUntilSpawnTime(timeOfDay.ToSeconds()));
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
            spawnDateTime.DespawnDate = spawnDateTime.DespawnDate.Validate();
        }
    }
}
