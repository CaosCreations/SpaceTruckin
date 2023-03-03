using Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewDayReportUI : MonoBehaviour
{
    [SerializeField] private GameObject reportCardInstance;
    [SerializeField] private NewDayReportCard reportCard;
    [SerializeField] private MissionModifierReportCard missionModifierReportCard;
    [SerializeField] private Text welcomeMessageText;

    private int currentReportIndex;

    private TerminalUIManager terminalManager;

    // We can only view the report once per day 
    public bool HasBeenViewedToday { get; set; }

    private List<ArchivedMission> MissionsToAppearInReport { get; set; }
    public ArchivedMission CurrentMissionToReport
    {
        get => MissionsToAppearInReport[currentReportIndex];
    }

    private void Awake()
    {
        CalendarManager.OnEndOfDay += () => HasBeenViewedToday = false;

        terminalManager = GetComponentInParent<TerminalUIManager>();
    }

    private void Start()
    {
        //SingletonManager.EventService.Add<OnModifierReportCardOpenedEvent>(OnModifierReportCardOpenedHandler);
    }

    private void OnEnable()
    {
        MissionsToAppearInReport = ArchivedMissionsManager.GetMissionsToAppearInReport();

        // Don't let them exit the report until all missions have been shown.
        UIManager.AddOverriddenKey(PlayerConstants.ExitKey);
    }

    private void OnDisable()
    {
        HasBeenViewedToday = true;
        MissionsToAppearInReport.Clear();
        UIManager.RemoveOverriddenKey(PlayerConstants.ExitKey);
    }

    public void Init()
    {
        reportCardInstance.SetActive(true);
        currentReportIndex = 0;
        reportCard.NextCardButton.SetText(UIConstants.NextCardText);
        UpdateNextCardButtonListener();
        ShowNextReport();

        // Insert Player Data in the welcome message, e.g. their name 
        welcomeMessageText.ReplaceTemplates();
    }

    private void ShowNextReport()
    {
        if (CurrentMissionToReport != null)
        {
            reportCard.gameObject.SetActive(true);
            reportCard.ShowReport(CurrentMissionToReport);
            CurrentMissionToReport.HasBeenViewedInReport = true;
            
            if (!CurrentMissionToReport.Mission.HasModifier || CurrentMissionToReport.ArchivedModifierOutcome.HasBeenViewedInReport)
            {
                UpdateNextCardButtonListener();
            }            
        }
    }

    private void UpdateNextCardButtonListener()
    {
        if (!CurrentMissionToReport.HasBeenViewedInReport 
            || (CurrentMissionToReport.Mission.HasModifier && !CurrentMissionToReport.ArchivedModifierOutcome.HasBeenViewedInReport))
        {
            reportCard.NextCardButton.AddOnClick(ShowNextReport);
            return;
        }

        // Cycle through to the next report card or add a CloseReport listener if we've reached the end
        if (currentReportIndex < MissionsToAppearInReport.Count - 1)
        {
            currentReportIndex++;
        }
        else
        {
            reportCard.NextCardButton.AddOnClick(CloseReport).SetText(UIConstants.CloseCardCycleText);
        }
    }

    private void OnModifierReportCardOpenedHandler(/*OnModifierReportCardOpenedEvent evt*/)
    {
        UpdateNextCardButtonListener();
    }

    private void CloseReport()
    {
        reportCardInstance.SetActive(false);
        gameObject.SetActive(false);
        terminalManager.MissionsPanel.SetActive(true);
        terminalManager.MissionsButton.SetColour(terminalManager.MissionsPanel.GetImageColour());

        // Allow the exit key to be used as normal now that the report has finished.
        UIManager.RemoveOverriddenKey(PlayerConstants.ExitKey);
    }

    private void Update()
    {
        if (reportCardInstance != null && Input.GetKeyDown(PlayerConstants.ExitKey))
        {
            CloseReport();
        }
    }
}
