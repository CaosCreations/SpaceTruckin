using Events;
using UnityEngine;

public class InGameHUDManager : MonoBehaviour
{
    public static InGameHUDManager Instance { get; private set; }

    [SerializeField]
    private GameObject hudCanvas;

    [SerializeField]
    private DateTimeText dateTimeText;

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
        return SceneLoadingManager.IsLoadedSceneName(SceneType.MainStation) && CalendarManager.CurrentDate != new Date(1, 1, 1);
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnSceneLoadedEvent>(OnSceneLoadedHandler);
        SingletonManager.EventService.Add<OnMorningStartEvent>(OnMorningStartHandler);
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

    private void OnMorningStartHandler()
    {
        SetActive();
    }
}
