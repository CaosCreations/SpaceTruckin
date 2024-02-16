﻿using Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewDayReportUI : MonoBehaviour
{
    [SerializeField] private NewDayReportCard reportCard;
    [SerializeField] private Text welcomeMessageText;

    public bool HasBeenViewedToday { get; set; }
    private int currentReportIndex;
    private List<ArchivedMission> MissionsToAppearInReport { get; set; }
    public ArchivedMission CurrentMissionToReport => MissionsToAppearInReport[currentReportIndex];

    private void Awake()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
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

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        HasBeenViewedToday = false;
    }

    public void Init()
    {
        reportCard.Init();
        reportCard.gameObject.SetActive(true);
        currentReportIndex = 0;
        reportCard.NextCardButton.SetText(UIConstants.NextCardText);
        UpdateNextCardButtonListener();
        ShowNextReport();
        welcomeMessageText.ReplaceTemplates();
    }

    private void ShowNextReport()
    {
        reportCard.gameObject.SetActive(true);
        reportCard.ShowReport(CurrentMissionToReport);
        CurrentMissionToReport.HasBeenViewedInReport = true;
        UpdateNextCardButtonListener();
    }

    private void UpdateNextCardButtonListener()
    {
        if (!CurrentMissionToReport.HasBeenViewedInReport)
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

    private void CloseReport()
    {
        reportCard.gameObject.SetActive(false);
        gameObject.SetActive(false);
        UIManager.TerminalManager.SwitchPanel(TerminalUIManager.Tab.Missions);
        UIManager.RemoveOverriddenKey(PlayerConstants.ExitKey);

        if (CalendarManager.CurrentDate == new Date(2, 1, 1))
        {
            UIManager.ShowTutorial(UICanvasType.Terminal);
        }
    }

    private void Update()
    {
        if (reportCard != null && reportCard.gameObject != null && Input.GetKeyDown(PlayerConstants.ExitKey))
        {
            CloseReport();
        }
    }
}
