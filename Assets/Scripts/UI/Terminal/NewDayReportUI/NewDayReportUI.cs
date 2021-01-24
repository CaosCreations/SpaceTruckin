using System.Linq;
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

    private void Start()
    {
        reportCardInstance = Instantiate(reportCardPrefab, transform);
        currentReportIndex = 0;
        ShowNextReport();
    }

    private void ShowNextReport()
    {
        if (MissionsManager.Instance.MissionsCompletedToday != null 
            && MissionsManager.Instance.MissionsCompletedToday.Any())
        {
            reportCard.ShowReport(CurrentMissionToReport);

            if (currentReportIndex < MissionsManager.Instance.MissionsCompletedToday.Count - 1)
            {
                currentReportIndex++;
            }
            reportCard.SetupNextCardListener(CurrentMissionToReport);
        }
    }
}
