using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCData npcData;

    public NPCData Data => npcData;

    private void Awake()
    {
        Data.LocationByDateContainer.InitLookup();
    }
}