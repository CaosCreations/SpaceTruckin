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
        Data.LocationByDateContainer.InitLookup();
    }
}