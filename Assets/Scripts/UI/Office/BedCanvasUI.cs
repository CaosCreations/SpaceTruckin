using Events;
using UnityEngine;

public class BedCanvasUI : UICanvasBase
{
    [SerializeField] private string popupText = "Time for bed?";

    private void OnEnable()
    {
        PopupManager.ShowPopup(OnSleep, () => { }, bodyText: popupText);
    }

    private void OnSleep()
    {
        PlayerManager.EnterPausedState();
        UIManager.ClearCanvases(false);
        SingletonManager.EventService.Dispatch(new OnPlayerSleepEvent());
    }
}
