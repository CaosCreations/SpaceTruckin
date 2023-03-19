using Events;
using TMPro;
using UnityEngine;

public class TimeOfDayTransitionUI : MonoBehaviour
{
    [SerializeField]
    private ImageOpacityTransition imageOpacityTransition;

    [SerializeField]
    private GameObject textCanvas;

    private TMP_Text text;

    private void Start()
    {
        SingletonManager.EventService.Add<OnEveningStartEvent>(OnEveningStartHandler);
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
        imageOpacityTransition.OnTransitionEnd += OnTransitionEndHandler;
        text = textCanvas.GetComponentInChildren<TMP_Text>();
    }

    private void BeginTransition(string textContent)
    {
        textCanvas.SetActive(true);
        imageOpacityTransition.SetActive(true);
        imageOpacityTransition.enabled = true;
        text.SetText(textContent);
        PlayerManager.EnterPausedState();
    }

    private void EndTransition()
    {
        textCanvas.SetActive(false);
        imageOpacityTransition.SetActive(false);
        PlayerManager.ExitPausedState();
    }

    private void OnEveningStartHandler()
    {
        BeginTransition(UIConstants.EveningStartText);
    }

    private void OnTransitionEndHandler()
    {
        EndTransition();
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        BeginTransition(UIConstants.MorningStartText);
    }
}