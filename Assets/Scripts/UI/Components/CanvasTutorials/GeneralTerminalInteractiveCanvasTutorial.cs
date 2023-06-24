using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GeneralTerminalInteractiveCanvasTutorial : InteractiveCanvasTutorial
{
    [SerializeField] private InteractiveCanvasTutorialCard fleetCard;
    [SerializeField] private InteractiveCanvasTutorialCard licencesCard;
    [SerializeField] private InteractiveCanvasTutorialCard analyticsCard;

    private bool fleetCardShown;
    private bool licencesCardShown;
    private bool analyticsCardShown;

    /// <summary>
    /// Tab button cards can be shown after a mission is scheduled with a pilot 
    /// </summary>
    private bool tabButtonCardsUnlocked;

    [SerializeField] private Button fleetButton;
    [SerializeField] private Button licencesButton;
    [SerializeField] private Button analyticsButton;

    protected override void Start()
    {
        base.Start();
        fleetButton.AddOnClick(FleetButtonHandler, removeListeners: false);
        licencesButton.AddOnClick(LicencesButtonHandler, removeListeners: false);
        analyticsButton.AddOnClick(AnalyticsButtonHandler, removeListeners: false);
        licencesCard.OnClosed += EndTutorial;
        SingletonManager.EventService.Add<OnPilotSlottedWithMissionEvent>(OnPilotSlottedWithMissionEventHandler);

        fleetCard.OnClosed += EndIfAllShown;
        licencesCard.OnClosed += EndIfAllShown;
        analyticsCard.OnClosed += EndIfAllShown;
    }

    private void ShowCard(InteractiveCanvasTutorialCard card, ref bool cardShown, Button button, UnityAction buttonHandler)
    {
        if (!tabButtonCardsUnlocked || cardShown)
            return;

        ShowCard(card);
        cardShown = true;
        button.onClick.RemoveListener(buttonHandler);
    }

    private void EndIfAllShown()
    {
        if (fleetCardShown && licencesCardShown && analyticsCardShown)
            EndTutorial();
    }

    private void FleetButtonHandler()
    {
        ShowCard(fleetCard, ref fleetCardShown, fleetButton, FleetButtonHandler);
    }

    private void LicencesButtonHandler()
    {
        ShowCard(licencesCard, ref licencesCardShown, licencesButton, LicencesButtonHandler);
    }

    private void AnalyticsButtonHandler()
    {
        ShowCard(analyticsCard, ref analyticsCardShown, analyticsButton, AnalyticsButtonHandler);
    }

    private void OnPilotSlottedWithMissionEventHandler()
    {
        if (tabButtonCardsUnlocked)
            return;

        Debug.Log("Unlocking tab button cards now pilot has been slotted with mission...");
        tabButtonCardsUnlocked = true;
        UnlockCanvas();
    }

    protected override void CloseAllCards()
    {
        fleetCard.SetActive(false);
        licencesCard.SetActive(false);
        analyticsCard.SetActive(false);
    }
}
