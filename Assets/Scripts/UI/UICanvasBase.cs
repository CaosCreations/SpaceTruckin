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

    public bool IsTutorialActive => CanvasTutorialsByDate != null
        && CanvasTutorialsByDate.Any(t => t != null && t.Tutorial != null && t.Tutorial.gameObject != null && t.Tutorial.gameObject.activeSelf);

    public bool TryGetTutorial(Date date, out InteractiveCanvasTutorial tutorial)
    {
        tutorial = CanvasTutorialsByDate.FirstOrDefault(t => t.Date == date)?.Tutorial;
        return tutorial != null;
    }

    private void ShowTutorial(InteractiveCanvasTutorial tutorial)
    {
        tutorial.SetActive(true);
        tutorial.Init(() => PlayerPrefsManager.SetCanvasTutorialPrefValue(CanvasType, CalendarManager.CurrentDate, true));
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

        ShowTutorial(tutorial);
    }
}
