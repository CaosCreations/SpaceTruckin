using Events;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    [SerializeField]
    private NPCDataContainer npcDataContainer;

    private NPC[] npcs;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        npcs = FindObjectsOfType<NPC>();
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnEveningStartEvent>(OnEveningStartHandler);
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);

        SetMorningPositions();
    }

    private void SetMorningPositions()
    {
        foreach (var npc in npcs)
        {
            if (npc.Data.Location == null || npc.Data.Location.MorningStationPosition == Vector3.zero)
                continue;

            npc.transform.position = npc.Data.Location.MorningStationPosition;
        }
    }

    private void SetEveningPositions()
    {
        foreach (var npc in npcs)
        {
            if (npc.Data.Location == null || npc.Data.Location.EveningStationPosition == Vector3.zero)
                continue;

            npc.transform.position = npc.Data.Location.EveningStationPosition;
        }
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        SetMorningPositions();
    }

    private void OnEveningStartHandler()
    {
        // Change positions of NPC's when evening starts each day
        SetEveningPositions();
    }
}