using Events;
using UnityEngine;
using UnityEngine.UI;

public class HangarNodeInteractiveCanvasTutorial : InteractiveCanvasTutorial
{
    [SerializeField] private InteractiveCanvasTutorialCard pilotDetailsCard;
    [SerializeField] private InteractiveCanvasTutorialCard moneyAndToolsCard;
    [SerializeField] private InteractiveCanvasTutorialCard fuelCard;
    [SerializeField] private InteractiveCanvasTutorialCard repairsCard;
    //[SerializeField] private InteractiveCanvasTutorialCard minigameCard1;
    //[SerializeField] private InteractiveCanvasTutorialCard minigameCard2;
    //[SerializeField] private InteractiveCanvasTutorialCard minigameCard3;
    //[SerializeField] private InteractiveCanvasTutorialCard minigameCard4;
    //[SerializeField] private InteractiveCanvasTutorialCard backToMainPanelCard;
    [SerializeField] private InteractiveCanvasTutorialCard customisationsCard;
    [SerializeField] private InteractiveCanvasTutorialCard returnToQueueCard;
    [SerializeField] private InteractiveCanvasTutorialCard startMissionCard;

    [SerializeField] private Button repairsButton;
    [SerializeField] private Button mainPanelButton;

    private bool repairsCardShown;
    //private bool minigameCard1Shown;
    //private bool minigameCard4Shown;
    private bool customisationsCardShown;

    protected override void Start()
    {
        base.Start();
        openingCard.OnClosed += () => ShowCard(pilotDetailsCard);
        pilotDetailsCard.OnClosed += () => ShowCard(moneyAndToolsCard);
        moneyAndToolsCard.OnClosed += () => ShowCard(fuelCard);
        //minigameCard1.OnClosed += () => ShowCard(minigameCard2);
        //minigameCard2.OnClosed += () => ShowCard(minigameCard3);
        //minigameCard4.OnClosed += () => ShowCard(backToMainPanelCard);
        customisationsCard.OnClosed += () => ShowCard(returnToQueueCard);
        returnToQueueCard.OnClosed += () => ShowCard(startMissionCard);

        repairsButton.AddOnClick(RepairsButtonHandler, removeListeners: false);
        mainPanelButton.AddOnClick(MainPanelButtonHandler, removeListeners: false);

        SingletonManager.EventService.Add<OnFuelingEndedEvent>(OnFuelingEndedEventHandler);
        //SingletonManager.EventService.Add<OnRepairsMinigameWonEvent>(OnRepairsMinigameWonEventHandler);
    }

    private void RepairsButtonHandler()
    {
        //ShowCard(minigameCard1, ref minigameCard1Shown, repairsButton, RepairsButtonHandler);
        ShowCard(customisationsCard, ref customisationsCardShown, repairsButton, RepairsButtonHandler);
    }

    private void MainPanelButtonHandler()
    {
        ShowCard(customisationsCard, ref customisationsCardShown, mainPanelButton, MainPanelButtonHandler);
    }

    private void OnFuelingEndedEventHandler()
    {
        if (repairsCardShown)
            return;

        ShowCard(repairsCard);
        repairsCardShown = true;
    }

    //private void OnRepairsMinigameWonEventHandler(OnRepairsMinigameWonEvent evt)
    //{
    //    if (minigameCard4Shown)
    //        return;

    //    ShowCard(minigameCard4);
    //    minigameCard4Shown = true;
    //}

    protected override void CloseAllCards()
    {
        pilotDetailsCard.SetActive(false);
        moneyAndToolsCard.SetActive(false);
        fuelCard.SetActive(false);
        repairsCard.SetActive(false);
        //minigameCard1.SetActive(false);
        //minigameCard2.SetActive(false);
        //minigameCard3.SetActive(false);
        //minigameCard4.SetActive(false);
        //backToMainPanelCard.SetActive(false);
        customisationsCard.SetActive(false);
    }
}