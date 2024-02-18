using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StationSetupManager : MonoBehaviour
{
    public static StationSetupManager Instance { get; private set; }

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
        if (SceneLoadingManager.GetCurrentSceneType() == SceneType.MainStation)
        {
            SetUpStation();
            return;
        }
        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            // When changing to main station scene 
            if (SceneLoadingManager.GetSceneNameByType(SceneType.MainStation) == next.name)
            {
                SetUpStation();
            }
        };
    }

    public static void SetUpStation()
    {
        Debug.Log("Setting up station...");
        CalendarManager.ResetCalendar();
        ClockManager.SetCurrentTime(CalendarManager.StationEntryTimeOfDay.ToSeconds(), overrideTransition: true);
        SingletonManager.Init();
        PlayerManager.Instance.SetUpPlayer();
        TimelineManager.Instance.SetUp();
        SingletonManager.EventService.Dispatch<OnStationSetUpEvent>();
        MissionsManager.Instance.SetUp();
        MusicManager.Instance.SetUp();
    }
}