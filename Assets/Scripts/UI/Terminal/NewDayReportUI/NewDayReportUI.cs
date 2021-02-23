using UnityEngine;

public class NewDayReportUI : MonoBehaviour
{
    public GameObject reportCardPrefab;
    public GameObject reportCardInstance; 
    public NewDayReportCard reportCard;
    private int currentReportIndex;

    private TerminalUIManager terminalManager;
    public bool HasBeenViewed { get; set; }
    public ArchivedMission CurrentMissionToReport 
    {
        get => ArchivedMissionsManager.Instance.MissionsCompletedYesterday[currentReportIndex];
    }

    private void Start()
    {
        BedCanvasUI.OnEndOfDay += () => HasBeenViewed = false;
        terminalManager = GetComponentInParent<TerminalUIManager>();
    } 
    
    private void OnDisable() => HasBeenViewed = true;

    public void Init()
    {
        reportCardInstance.SetActive(true);
        currentReportIndex = 0;
        ShowNextReport();
    }

    private void ShowNextReport()
    {
        if (CurrentMissionToReport != null)
        {
            reportCard.ShowReport(CurrentMissionToReport);

            if (currentReportIndex < ArchivedMissionsManager.Instance
                .MissionsCompletedYesterday.Count - 1)
            {
                currentReportIndex++;
                reportCard.SetupNextCardListener(CurrentMissionToReport);
            }
            else
            {
                reportCard.nextCardButton.AddOnClick(CloseReport).SetText("Close");
            }
        }
    }

    private void CloseReport()
    {
        reportCardInstance.SetActive(false);
        gameObject.SetActive(false);
        terminalManager.missionsPanel.SetActive(true);
        terminalManager.missionsButton.SetColour
            (terminalManager.missionsPanel.GetImageColour());
    }

    private void Update()
    {
        if (reportCardInstance != null && Input.GetKeyDown(PlayerConstants.exit))
        {
            CloseReport();
        }
    }
}
