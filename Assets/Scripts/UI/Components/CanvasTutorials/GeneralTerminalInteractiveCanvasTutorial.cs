using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GeneralTerminalInteractiveCanvasTutorial : InteractiveCanvasTutorial
{
    [SerializeField] private InteractiveCanvasTutorialCard fleetCard;
    [SerializeField] private InteractiveCanvasTutorialCard licencesCard;
    [SerializeField] private InteractiveCanvasTutorialCard analyticsCard;
    [SerializeField] private InteractiveCanvasTutorialCard cantExitCard;

    private bool fleetCardShown;
    private bool licencesCardShown;
    private bool analyticsCardShown;

    [SerializeField] private Button fleetButton;
    [SerializeField] private Button licencesButton;
    [SerializeField] private Button analyticsButton;

    [SerializeField] private UniversalUI universalUI;

    private void Start()
    {
        fleetButton.AddOnClick(FleetButtonHandler, removeListeners: false);
        licencesButton.AddOnClick(LicencesButtonHandler, removeListeners: false);
        analyticsButton.AddOnClick(AnalyticsButtonHandler, removeListeners: false);
        universalUI.AddCloseWindowButtonListener(CloseTerminalButtonHandler);
        licencesCard.OnClosed += EndTutorial;
    }

    private void EndIfAllShown()
    {
        if (fleetCardShown && licencesCardShown && analyticsCardShown)
            EndTutorial();
    }

    private void ShowCard(InteractiveCanvasTutorialCard card, ref bool cardShown, Button button, UnityAction buttonHandler)
    {
        ShowCard(card);
        cardShown = true;
        button.onClick.RemoveListener(buttonHandler);
        EndIfAllShown();
    }

    private void ShowCard(InteractiveCanvasTutorialCard card)
    {
        CloseAllCards();
        card.SetActive(true);
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

    private void CloseTerminalButtonHandler()
    {
        ShowCard(cantExitCard);
    }

    private void CloseAllCards()
    {
        fleetCard.SetActive(false);
        licencesCard.SetActive(false);
        analyticsCard.SetActive(false);
    }

    protected override void EndTutorial()
    {
        base.EndTutorial();
        universalUI.RemoveCloseWindowButtonListener(CloseTerminalButtonHandler);
        universalUI.EnableCloseWindowButton();
    }
}
