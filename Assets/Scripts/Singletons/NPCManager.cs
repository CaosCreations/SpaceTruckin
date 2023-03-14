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

        Instance.npcs = FindObjectsOfType<NPC>();
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnEveningStartEvent>(OnEveningStartHandler);
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
    }

    private void SetMorningPositions()
    {
        foreach (var npc in npcs)
        {
            npc.transform.position = npc.Data.Location.MorningStationPosition;
        }
    }

    private void SetEveningPositions()
    {
        foreach (var npc in npcs)
        {
            npc.transform.position = npc.Data.Location.EveningStationPosition;
        }
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        SetMorningPositions();
    }

    private void OnEveningStartHandler(OnEveningStartEvent evt)
    {
        // Change positions of NPC's when evening starts each day
        SetEveningPositions();
    }
}