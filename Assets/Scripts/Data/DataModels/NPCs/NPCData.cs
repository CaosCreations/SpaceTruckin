using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "ScriptableObjects/NPCData", order = 1)]
public class NPCData : ScriptableObject
{
    [Tooltip("Where the NPC appears by default in the morning and evening respectively")]
    [field: SerializeField]
    public NPCLocation Location { get; private set; } = new();

    [Tooltip("Where the NPC appears by default in the morning and evening on specific dates")]
    [field: SerializeField]
    public NPCLocationsByDate LocationsByDate { get; private set; } = new();
}