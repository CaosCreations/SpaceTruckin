using Events;
using System.Linq;
using UnityEngine;

public class BedCanvasUI : UICanvasBase
{
    [SerializeField] private string popupText = "Time for bed?";
    [SerializeField] private Cutscene[] endOfDayCutscenes;

    private void OnEnable()
    {
        PopupManager.ShowPopup(Sleep, () => { }, bodyText: popupText);
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
    }

    private void Sleep()
    {
        PlayerManager.EnterPausedState();
        UIManager.ClearCanvases(false);
        SingletonManager.EventService.Dispatch(new OnPlayerSleepEvent());
    }

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        if (endOfDayCutscenes != null && endOfDayCutscenes.Contains(evt.Cutscene))
        {
            Sleep();
        }
    }
}
