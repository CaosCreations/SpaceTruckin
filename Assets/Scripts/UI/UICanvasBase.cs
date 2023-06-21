using System.Linq;
using UnityEngine;

public class UICanvasBase : MonoBehaviour
{
    [field: SerializeField]
    public UICanvasType CanvasType { get; private set; }

    [field: SerializeField]
    public CanvasTutorialPrefabByDate[] CanvasTutorialsByDate { get; private set; }

    [field: SerializeField]
    public bool ShowUniversalCanvas { get; private set; }

    [field: SerializeField]
    public CameraZoomSettings CameraZoomSettings { get; private set; }

    [field: SerializeField]
    public bool ZoomInBeforeOpening { get; private set; }

    private bool TryGetTutorial(Date date, out GameObject prefab)
    {
        prefab = CanvasTutorialsByDate.FirstOrDefault(t => t.Date == date)?.Prefab;
        return prefab != null;
    }

    public virtual void ShowTutorialIfExistsAndUnseen()
    {
        // Is there a tutorial to show for this date?
        if (!TryGetTutorial(CalendarManager.CurrentDate, out var prefab))
        {
            return;
        }

        // Have we seen it already?
        if (PlayerPrefsManager.GetCanvasTutorialPrefValue(CanvasType, CalendarManager.CurrentDate))
        {
            return;
        }

        Instantiate(prefab, transform);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(CanvasType, CalendarManager.CurrentDate, true);
    }
}
