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
        imageOpacityTransition.OnTransitionEnd += OnTransitionEndHandler;
        text = textCanvas.GetComponentInChildren<TMP_Text>();
    }

    private void OnEveningStartHandler()
    {
        textCanvas.SetActive(true);
        imageOpacityTransition.SetActive(true);
        imageOpacityTransition.enabled = true;
        text.SetText(UIConstants.EveningStartText);
        PlayerManager.EnterPausedState();
    }

    private void OnTransitionEndHandler()
    {
        textCanvas.SetActive(false);
        imageOpacityTransition.SetActive(false);
        PlayerManager.ExitPausedState();
    }
}