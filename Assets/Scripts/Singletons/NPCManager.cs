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
            Vector3 position;
            if (npc.Data.LocationsByDate.TryGetValue(CalendarManager.CurrentDate, out var locationByDate)) 
            {
                position = locationByDate.MorningStationPosition;
            }
            else
            {
                position = npc.Data.Location.MorningStationPosition;
            }

            if (position == Vector3.zero)
                continue;

            npc.transform.position = position;
        }
    }

    private void SetEveningPositions()
    {
        foreach (var npc in npcs)
        {
            Vector3 position;
            if (npc.Data.LocationsByDate.TryGetValue(CalendarManager.CurrentDate, out var locationByDate))
            {
                position = locationByDate.EveningStationPosition;
            }
            else
            {
                position = npc.Data.Location.EveningStationPosition;
            }

            if (position == Vector3.zero)
                continue;

            npc.transform.position = position;
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