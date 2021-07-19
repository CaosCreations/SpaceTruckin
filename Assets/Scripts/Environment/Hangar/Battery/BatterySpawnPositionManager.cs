using UnityEngine;

public class BatterySpawnPositionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPositions;

    public void Awake()
    {
        // As the game starts, spawn the batteries at their initial position

        if (HangarManager.BatteryWrappers.Length > spawnPositions.Length)
        {
            Debug.LogError("As we start the game, there aren't enough spawn positions for all batteries. We need to add more.");
        }

        for (int i = 0; i < HangarManager.BatteryWrappers.Length; i++)
        {
            if (HangarManager.BatteryWrappers[i] == null)
                continue;

            HangarManager.BatteryWrappers[i].BatteryInteractable.transform.position = spawnPositions[i].transform.position;
        }
    }

    // Respawn the battery when the player tries to take it out of the hangar
    public void RespawnBattery(Transform objectToMove, Collider boundaries)
    {
        Collider[] colliders;

        // We loop through the spawn positions until we find one that is free
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            // Check if there is already a gameobject within the space where we want to spawn the battery
            colliders = Physics.OverlapBox(spawnPositions[i].transform.position, boundaries.bounds.extents, Quaternion.identity);

            // If Physics.OverlapBox() returns nothing, it means that the space is free
            if (colliders.Length == 0)
            {
                objectToMove.position = spawnPositions[i].transform.position;
                return;
            }
        }

        Debug.LogError("There are no available battery spawn positions. We need to add more.");
    }
}
