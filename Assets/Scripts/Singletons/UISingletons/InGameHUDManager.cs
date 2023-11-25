using Events;
using UnityEngine;

public class InGameHUDManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hudCanvas;

    [SerializeField]
    private DateTimeText dateTimeText;

    [SerializeField]
    private Cutscene dateTimeUIActivateCutscene;

    private bool IsDateTimeUIActive()
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

    private void SetActive(bool active)
    {
        hudCanvas.SetActive(active);
        dateTimeText.SetActive(active);
    }

    private void SetActive()
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
