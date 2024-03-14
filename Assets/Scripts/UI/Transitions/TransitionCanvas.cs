using Events;
using TMPro;
using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private ImageOpacityTransition imageOpacityTransition;

    [SerializeField]
    private TMP_Text text;

    [field: SerializeField]
    public TransitionUI.TransitionType TransitionType { get; private set; }

    public bool IsTransitioning { get; private set; }

    private void Awake()
    {
        imageOpacityTransition.OnTransitionEnd += EndTransition;
        imageOpacityTransition.OnTransitionMidpointReached += ReachTransitionMidpoint;
    }

    public void BeginTransition(string textContent)
    {
        IsTransitioning = true;
        canvas.gameObject.SetActive(true);
        imageOpacityTransition.SetActive(true);
        imageOpacityTransition.enabled = true;
        text.SetText(textContent);

        // Currently assumes transitions are not concurrent 
        PlayerManager.EnterPausedState();
        UIManager.AddOverriddenKey(KeyCode.Escape);
        SingletonManager.EventService.Dispatch(new OnUITransitionStartedEvent(TransitionType));
    }

    public void ReachTransitionMidpoint()
    {
        SingletonManager.EventService.Dispatch(new OnUITransitionMidpointReachedEvent(TransitionType));
    }

    public void EndTransition()
    {
        canvas.gameObject.SetActive(false);
        imageOpacityTransition.SetActive(false);
        PlayerManager.ExitPausedState();
        UIManager.RemoveOverriddenKey(KeyCode.Escape);
        SingletonManager.EventService.Dispatch(new OnUITransitionEndedEvent(TransitionType));
        IsTransitioning = false;
    }
}