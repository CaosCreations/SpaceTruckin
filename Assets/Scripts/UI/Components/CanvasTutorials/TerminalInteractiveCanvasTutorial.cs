using Events;
using UnityEngine;

public class TerminalInteractiveCanvasTutorial : InteractiveCanvasTutorial
{
    [SerializeField] private InteractiveCanvasTutorialCard cardAfterMissionSelected;
    [SerializeField] private InteractiveCanvasTutorialCard cardAfterPilotSelected;

    private bool cardAfterMissionSelectedShown;
    private bool cardAfterPilotSelectedShown;

    private void Start()
    {
        SingletonManager.EventService.Add<OnMissionSlottedEvent>(OnMissionSlottedHandler);
        SingletonManager.EventService.Add<OnPilotSlottedEvent>(OnPilotSlottedHandler);
        cardAfterPilotSelected.AddCloseButtonListener(EndTutorial);
    }

    private void OnPilotSlottedHandler()
    {
        if (cardAfterPilotSelectedShown)
            return;

        cardAfterPilotSelected.SetActive(true);
        cardAfterPilotSelectedShown = true;
    }

    private void OnMissionSlottedHandler()
    {
        if (cardAfterMissionSelectedShown)
            return;

        cardAfterMissionSelected.SetActive(true);
        cardAfterMissionSelectedShown = true;
    }
}