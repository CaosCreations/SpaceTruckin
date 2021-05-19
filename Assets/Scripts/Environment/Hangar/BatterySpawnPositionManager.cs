using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawnPositionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPositions;

    [SerializeField] private GameObject[] batteries;

    public void Awake()
    {
        for (int i = 0; i < batteries.Length; i++)
        {
            batteries[i].transform.position = spawnPositions[i].transform.position;
        }
    }

    public void RespawnBattery(Transform objectToMove, Collider boundaries)
    {
        Collider[] colliders;

        // We loop through the spawn positions until we find one that is free
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            // Check if there is already a gameobject within the space where we want to spawn the battery
            colliders = Physics.OverlapBox(spawnPositions[i].transform.position, boundaries.bounds.extents, Quaternion.identity);

            // If Physics.OverlapBox() returns nothing, it means that the space is free
            if(colliders.Length == 0)
            {
                objectToMove.transform.position = spawnPositions[i].transform.position;
                return;
            }
        }

        Debug.LogError("There are no available battery spawn positions. We need to add more.");
    }
}
