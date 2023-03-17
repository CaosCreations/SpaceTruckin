using UnityEngine;

public class UICanvasBase : MonoBehaviour
{
    [field: SerializeField] 
    public GameObject CanvasTutorialPrefab { get; private set; }

    [field: SerializeField] 
    public bool ShowUniversalCanvas { get; private set; }

    [field: SerializeField]
    public CameraZoomSettings CameraZoomSettings { get; private set; }

    [field: SerializeField]
    public bool ZoomInBeforeOpening { get; private set; }

    public virtual void ShowTutorial()
    {
        if (CanvasTutorialPrefab != null)
        {
            GameObject tutorial = Instantiate(CanvasTutorialPrefab, transform);
            
            if (tutorial.TryGetComponent<CardCycle>(out var cardCycle))
            {
                cardCycle.SetupCardCycle();
            }
        }
    }
}