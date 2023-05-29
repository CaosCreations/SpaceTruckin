using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCData npcData;

    public NPCData Data => npcData;

    [SerializeField]
    private NPCAnimated npcAnimated;

    public NPCAnimated Animated => npcAnimated;

    private void Awake()
    {
        if (Data == null)
        {
            Debug.LogWarning($"{gameObject.name} NPC has no {nameof(NPCData)} ScriptableObject reference assigned");
            return;
        }
        Data.LocationByDateContainer.InitLookup();
    }

    public override string ToString()
    {
        return $"NPC with name '{gameObject.name}'";
    }
}