using Events;
using UnityEngine;

public class MissionTerminalInteractiveCanvasTutorial : InteractiveCanvasTutorial
{
    [SerializeField] private InteractiveCanvasTutorialCard cardAfterMissionsListClicked;
    [SerializeField] private InteractiveCanvasTutorialCard cardAfterMissionSelected;
    [SerializeField] private InteractiveCanvasTutorialCard cardAfterPilotSelected;
    private GameObject missionsListTarget;

    protected override void Start()
    {
        base.Start();
        SingletonManager.EventService.Add<OnMissionSlottedEvent>(OnMissionSlottedHandler);
        SingletonManager.EventService.Add<OnPilotSlottedWithMissionEvent>(OnPilotSlottedWithMissionHandler);
        SingletonManager.EventService.Add<OnPilotSelectClosedEvent>(OnPilotSelectClosedHandler);
    }

    protected override void OnEnable()
    {
        UIManager.TerminalManager.SetSingleTabButtonInteractable(TerminalUIManager.Tab.Missions);
        missionsListTarget = GameObject.FindGameObjectWithTag(MissionConstants.MissionsListRaycastTag);
        missionsListTarget.SetActive(true);
        base.OnEnable();
    }

    private void Update()
    {
        if (openingCard.gameObject.activeSelf && Input.GetMouseButtonDown(0) && UIUtils.IsPointerOverTag(MissionConstants.MissionsListRaycastTag))
        {
            ShowCard(cardAfterMissionsListClicked);

            if (missionsListTarget != null)
            {
                Destroy(missionsListTarget);
            }
        }
    }

    protected override void EndTutorial()
    {
        UIManager.TerminalManager.SetTabButtonsInteractable(true);
        base.EndTutorial();
    }

    private void OnPilotSelectClosedHandler()
    {
        if (cardAfterMissionSelected.gameObject.activeSelf)
        {
            ToPreviousCard();
        }
    }

    private void OnMissionSlottedHandler()
    {
        OnSlottedHandler(cardAfterMissionSelected);
    }

    private void OnPilotSlottedWithMissionHandler()
    {
        OnSlottedHandler(cardAfterPilotSelected);
    }

    private void OnSlottedHandler(InteractiveCanvasTutorialCard card)
    {
        if (seenCards.Contains(card))
            return;

        ShowCard(card);
    }

    protected override void CloseAllCards()
    {
        openingCard.SetActive(false);
        cardAfterMissionSelected.SetActive(false);
        cardAfterPilotSelected.SetActive(false);
    }
}
