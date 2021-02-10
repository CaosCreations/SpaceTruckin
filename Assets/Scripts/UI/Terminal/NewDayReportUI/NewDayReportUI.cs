using UnityEngine;

public class NewDayReportUI : MonoBehaviour
{
    public GameObject reportCardPrefab;
    public GameObject reportCardInstance; 
    public NewDayReportCard reportCard;
    private int currentReportIndex;

    public GameObject missionsPanel;
    public bool HasBeenViewed { get; set; }

    public ArchivedMission CurrentMissionToReport 
    {
        get => ArchivedMissionsManager.Instance.MissionsCompletedYesterday[currentReportIndex];
    }

    private void Start()
    {
        BedCanvasUI.OnEndOfDay += () => HasBeenViewed = false;
        //reportCard = reportCardInstance.GetComponent<NewDayReportCard>();
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
                reportCard.nextCardButton.SetText("Close");
                reportCard.nextCardButton.AddOnClick(CloseReport);
            }
        }
    }

    private void CloseReport()
    {
        reportCardInstance.SetActive(false);
        gameObject.SetActive(false);
        missionsPanel.SetActive(true);
    }

    private void Update()
    {
        if (reportCardInstance != null && Input.GetKeyDown(PlayerConstants.exit))
        {
            CloseReport();
        }
    }
}
