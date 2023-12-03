using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Tab = TerminalUIManager.Tab;

public class GeneralTerminalInteractiveCanvasTutorial : InteractiveCanvasTutorial
{
    [SerializeField] private InteractiveCanvasTutorialCard messagesCard;
    [SerializeField] private InteractiveCanvasTutorialCard analyticsCard;
    [SerializeField] private InteractiveCanvasTutorialCard fleetCard;
    [SerializeField] private InteractiveCanvasTutorialCard licencesCard;
    [SerializeField] private InteractiveCanvasTutorialCard scheduledMissionsCard;
    [SerializeField] private InteractiveCanvasTutorialCard afterScheduledMissionsCard;

    private bool missionsCardShown;
    private bool messagesCardShown;
    private bool fleetCardShown;
    private bool licencesCardShown;
    private bool analyticsCardShown;

    [SerializeField] private Button missionsButton;
    [SerializeField] private Button messagesButton;
    [SerializeField] private Button fleetButton;
    [SerializeField] private Button licencesButton;
    [SerializeField] private Button analyticsButton;

    protected override void Start()
    {
        base.Start();
        missionsButton.AddOnClick(MissionsButtonHandler, removeListeners: false);
        messagesButton.AddOnClick(MessagesButtonHandler, removeListeners: false);
        fleetButton.AddOnClick(FleetButtonHandler, removeListeners: false);
        licencesButton.AddOnClick(LicencesButtonHandler, removeListeners: false);
        analyticsButton.AddOnClick(AnalyticsButtonHandler, removeListeners: false);

        UIManager.TerminalManager.SetSingleTabButtonInteractable(Tab.Messages);
        SingletonManager.EventService.Add<OnPilotSlottedWithMissionEvent>(OnPilotSlottedWithMissionEventHandler);
    }

    protected override void ShowCard(InteractiveCanvasTutorialCard card, ref bool cardShown, Button button, UnityAction buttonHandler)
    {
        if (cardShown)
            return;

        ShowCard(card);
        cardShown = true;
        button.onClick.RemoveListener(buttonHandler);
    }

    private void MissionsButtonHandler()
    {
        if (licencesCardShown)
        {
            ShowCard(scheduledMissionsCard, ref missionsCardShown, missionsButton, MissionsButtonHandler);
            UIManager.TerminalManager.SetSingleTabButtonInteractable(Tab.Missions);
        }
    }

    private void MessagesButtonHandler()
    {
        ShowCard(messagesCard, ref messagesCardShown, messagesButton, MessagesButtonHandler);
        UIManager.TerminalManager.SetSingleTabButtonInteractable(Tab.Analytics);
    }

    private void AnalyticsButtonHandler()
    {
        ShowCard(analyticsCard, ref analyticsCardShown, analyticsButton, AnalyticsButtonHandler);
        UIManager.TerminalManager.SetSingleTabButtonInteractable(Tab.Fleet);
    }

    private void FleetButtonHandler()
    {
        ShowCard(fleetCard, ref fleetCardShown, fleetButton, FleetButtonHandler);
        UIManager.TerminalManager.SetSingleTabButtonInteractable(Tab.Licences);
    }

    private void LicencesButtonHandler()
    {
        ShowCard(licencesCard, ref licencesCardShown, licencesButton, LicencesButtonHandler);
        UIManager.TerminalManager.SetSingleTabButtonInteractable(Tab.Missions);
    }

    private void OnPilotSlottedWithMissionEventHandler()
    {
        if (HangarManager.AreAllSlotsOccupied())
        {
            ShowCard(afterScheduledMissionsCard);
            UnlockCanvas();
        }
    }

    protected override void CloseAllCards()
    {
        openingCard.SetActive(false);
        messagesCard.SetActive(false);
        analyticsCard.SetActive(false);
        fleetCard.SetActive(false);
        licencesCard.SetActive(false);
        scheduledMissionsCard.SetActive(false);
        afterScheduledMissionsCard.SetActive(false);
    }
}
