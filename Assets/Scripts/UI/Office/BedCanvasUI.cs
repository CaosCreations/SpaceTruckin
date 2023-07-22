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
        UIManager.ClearCanvases(false);
        SingletonManager.EventService.Dispatch(new OnPlayerSleepEvent());
    }
}
