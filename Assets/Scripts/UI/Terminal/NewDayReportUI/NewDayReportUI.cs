using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewDayReportUI : MonoBehaviour
{
    [SerializeField] private GameObject reportCardPrefab;
    [SerializeField] private GameObject reportCardInstance; 
    [SerializeField] private NewDayReportCard reportCard;
    [SerializeField] private Text welcomeMessageText;

    private Button nextCardButton;
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
        nextCardButton = GetComponentInChildren<Button>(includeInactive: true);
    }

    private void OnEnable()
    {
        MissionsToAppearInReport = ArchivedMissionsManager.GetMissionsToAppearInReport();
    }

    private void OnDisable() => HasBeenViewedToday = true;

    public void Init()
    {
        reportCardInstance.SetActive(true);
        currentReportIndex = 0;
        reportCard.nextCardButton.SetText(UIConstants.NextCardText);
        nextCardButton.AddOnClick(ShowNextReport);
        nextCardButton.onClick.Invoke();
        
        // Insert Player Data in the welcome message, e.g. their name 
        welcomeMessageText.ReplaceTemplates();
    }

    private void ShowNextReport()
    {
        if (CurrentMissionToReport != null)
        {
            reportCard.ShowReport(CurrentMissionToReport);

            CurrentMissionToReport.HasBeenViewedInReport = true;

            if (currentReportIndex < MissionsToAppearInReport.Count - 1)
            {
                currentReportIndex++;
            }
            else
            {
                reportCard.nextCardButton.AddOnClick(CloseReport).SetText(UIConstants.CloseCardCycleText);
            }
        }
    }

    private void CloseReport()
    {
        reportCardInstance.SetActive(false);
        gameObject.SetActive(false);
        terminalManager.MissionsPanel.SetActive(true);
        terminalManager.MissionsButton.SetColour(terminalManager.MissionsPanel.GetImageColour());
    }

    private void Update()
    {
        if (reportCardInstance != null && Input.GetKeyDown(PlayerConstants.ExitKey))
        {
            CloseReport();
        }
    }
}
