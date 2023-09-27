using Events;
using UnityEngine;

public class BedCanvasUI : UICanvasBase
{
    [SerializeField] private string popupText = "Time for bed?";

    private void OnEnable()
    {
        PopupManager.ShowPopup(Bed.Sleep, () => { }, bodyText: popupText);
    }
}
