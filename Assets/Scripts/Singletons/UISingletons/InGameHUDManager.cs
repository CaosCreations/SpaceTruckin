using Events;
using UnityEngine;

public class InGameHUDManager : MonoBehaviour
{
    public static InGameHUDManager Instance { get; private set; }

    [SerializeField]
    private GameObject hudCanvas;

    [SerializeField]
    private DateTimeText dateTimeText;

    [SerializeField]
    private Cutscene dateTimeUIActivateCutscene;

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

    private static bool IsDateTimeUIActive()
    {
        return SceneLoadingManager.IsSceneLoaded(SceneType.MainStation) && !CalendarManager.IsTimeFrozenToday;
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnSceneLoadedEvent>(OnSceneLoadedHandler);
        SingletonManager.EventService.Add<OnStationSetUpEvent>(OnStationSetUpHandler);
        SingletonManager.EventService.Add<OnMorningStartEvent>(OnMorningStartHandler);
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
        SetActive();
    }

    private static void SetActive(bool active)
    {
        Instance.hudCanvas.SetActive(active);
        Instance.dateTimeText.SetActive(active);
    }

    private static void SetActive()
    {
        SetActive(IsDateTimeUIActive());
    }

    private void OnSceneLoadedHandler(OnSceneLoadedEvent evt)
    {
        SetActive();
    }

    private void OnStationSetUpHandler()
    {
        SetActive();
    }

    private void OnMorningStartHandler()
    {
        SetActive();
    }

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        if (evt.Cutscene == dateTimeUIActivateCutscene)
        {
            SetActive(true);
        }
    }
}
