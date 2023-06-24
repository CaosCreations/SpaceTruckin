﻿using Events;
using UnityEngine;

public class MissionTerminalInteractiveCanvasTutorial : InteractiveCanvasTutorial
{
    [SerializeField] private InteractiveCanvasTutorialCard cardAfterMissionSelected;
    [SerializeField] private InteractiveCanvasTutorialCard cardAfterPilotSelected;

    private bool cardAfterMissionSelectedShown;
    private bool cardAfterPilotSelectedShown;

    private void Start()
    {
        SingletonManager.EventService.Add<OnMissionSlottedEvent>(OnMissionSlottedHandler);
        SingletonManager.EventService.Add<OnPilotSlottedWithMissionEvent>(OnPilotSlottedWithMissionHandler);
        cardAfterPilotSelected.OnClosed += EndTutorial;
    }

    private void OnMissionSlottedHandler()
    {
        OnSlottedHandler(cardAfterMissionSelected, ref cardAfterMissionSelectedShown);
    }

    private void OnPilotSlottedWithMissionHandler()
    {
        OnSlottedHandler(cardAfterPilotSelected, ref cardAfterPilotSelectedShown);
    }

    private void OnSlottedHandler(InteractiveCanvasTutorialCard card, ref bool shownFlag)
    {
        if (shownFlag)
            return;

        CloseAllCards();
        card.SetActive(true);
        shownFlag = true;
    }

    protected override void CloseAllCards()
    {
        openingCard.SetActive(false);
        cardAfterMissionSelected.SetActive(false);
        cardAfterPilotSelected.SetActive(false);
    }
}
