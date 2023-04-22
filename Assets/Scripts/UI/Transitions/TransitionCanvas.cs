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

    private void Awake()
    {
        imageOpacityTransition.OnTransitionEnd += EndTransition;
    }

    public void BeginTransition(string textContent)
    {
        canvas.gameObject.SetActive(true);
        imageOpacityTransition.SetActive(true);
        imageOpacityTransition.enabled = true;
        text.SetText(textContent);
        PlayerManager.EnterPausedState();
        SingletonManager.EventService.Dispatch(new OnUITransitionStartedEvent(TransitionType));
    }

    public void EndTransition()
    {
        canvas.gameObject.SetActive(false);
        imageOpacityTransition.SetActive(false);
        PlayerManager.ExitPausedState();
        SingletonManager.EventService.Dispatch(new OnUITransitionEndedEvent(TransitionType));
    }
}