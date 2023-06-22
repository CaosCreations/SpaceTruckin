using System.Linq;
using UnityEngine;

public class UICanvasBase : MonoBehaviour
{
    [field: SerializeField]
    public UICanvasType CanvasType { get; private set; }

    [field: SerializeField]
    public CanvasTutorialByDate[] CanvasTutorialsByDate { get; private set; }

    [field: SerializeField]
    public bool ShowUniversalCanvas { get; private set; }

    [field: SerializeField]
    public CameraZoomSettings CameraZoomSettings { get; private set; }

    [field: SerializeField]
    public bool ZoomInBeforeOpening { get; private set; }

    private bool TryGetTutorial(Date date, out InteractiveCanvasTutorial tutorial)
    {
        tutorial = CanvasTutorialsByDate.FirstOrDefault(t => t.Date == date)?.Tutorial;
        return tutorial != null;
    }

    public virtual void ShowTutorialIfExistsAndUnseen()
    {
        // Is there a tutorial to show for this date?
        if (!TryGetTutorial(CalendarManager.CurrentDate, out var tutorial))
        {
            return;
        }

        // Have we seen it already?
        if (PlayerPrefsManager.GetCanvasTutorialPrefValue(CanvasType, CalendarManager.CurrentDate))
        {
            return;
        }

        tutorial.SetActive(true);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(CanvasType, CalendarManager.CurrentDate, true);
    }
}
