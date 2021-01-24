using UnityEngine;

public class NewDayReportUI : MonoBehaviour
{
    public GameObject reportCardPrefab;
    public GameObject reportCardInstance; 
    private NewDayReportCard reportCard;
    private int currentReportIndex;

    public Mission CurrentMissionToReport 
    {
        get => MissionsManager.Instance.MissionsCompletedToday[currentReportIndex];
    }
    public Mission NextMission { get; set; }

    private void Start()
    {
        reportCardInstance = Instantiate(reportCardPrefab, transform);
        currentReportIndex = 0;
    }

    private void ShowNextReport()
    {
        reportCard.ShowReport(CurrentMissionToReport);

        if (currentReportIndex < MissionsManager.Instance.MissionsCompletedToday.Count - 1)
        {
            currentReportIndex++;
        }
    }
}
