﻿using Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewDayReportUI : MonoBehaviour
{
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
        SingletonManager.EventService.Add<OnModifierReportCardClosedEvent>(OnModifierReportCardClosedHandler);
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
        reportCard.gameObject.SetActive(true);
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

    private void OnModifierReportCardClosedHandler()
    {
        if (MissionsToAppearInReport.All(m => m.HasBeenViewedInReport))
        {
            terminalManager.SwitchPanel(TerminalUIManager.Tab.Missions);
        }
        else
        {
            ShowNextReport();
        }
    }

    private void CloseReport()
    {
        reportCard.gameObject.SetActive(false);
        gameObject.SetActive(false);
        terminalManager.SwitchPanel(TerminalUIManager.Tab.Missions);

        // Allow the exit key to be used as normal now that the report has finished.
        UIManager.RemoveOverriddenKey(PlayerConstants.ExitKey);
    }

    private void Update()
    {
        if (reportCard != null && reportCard.gameObject != null && Input.GetKeyDown(PlayerConstants.ExitKey))
        {
            CloseReport();
        }
    }
} 