using Events;
using UnityEngine;

public class BedCanvasUI : UICanvasBase
{
    [SerializeField]
    private ImageOpacityTransition imageOpacityTransition;

    private void Awake()
    {
        imageOpacityTransition.OnTransitionEnd += OnTransitionEndHandler;
    }

    private void OnEnable()
    {
        imageOpacityTransition.enabled = true;
    }

    private void OnTransitionEndHandler()
    {
        // Reset UI after night-day image transition finishes 
        UIManager.ClearCanvases();

        SingletonManager.EventService.Dispatch(new OnPlayerSleepEvent());
    }
}
