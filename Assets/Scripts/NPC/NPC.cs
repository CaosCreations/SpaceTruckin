using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCData npcData;

    public NPCData Data => npcData;
}