using UnityEngine;
using Events;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance { get; private set; }
    
    private readonly EventService eventService = new EventService();
    public static EventService EventService => Instance.eventService;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitSingletons();
        CalendarManager.OnEndOfDay += SaveAllSingletonData;
    }

    private void InitSingletons()
    {
        MissionsManager.Instance.Init();
        PilotsManager.Instance.Init();
        ArchivedMissionsManager.Instance.Init();
        PilotAssetsManager.Instance.Init();
        ShipsManager.Instance.Init();
        HangarManager.Instance.Init();
        MessagesManager.Instance.Init();
        PlayerManager.Instance.Init();
        CalendarManager.Instance.Init();
    }

    public static void SaveAllSingletonData()
    {
        PlayerManager.Instance.SaveData();
        MissionsManager.Instance.SaveData();
        ArchivedMissionsManager.Instance.SaveData();
        PilotsManager.Instance.SaveData();
        ShipsManager.Instance.SaveData();
        HangarManager.Instance.SaveBatteryData();
        MessagesManager.Instance.SaveData();
        LicencesManager.Instance.SaveData();
        CalendarManager.Instance.SaveData();
    }
}
