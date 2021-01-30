using System.Linq;
using UnityEngine;

public class NewDayReportUI : MonoBehaviour
{
    public GameObject reportCardPrefab;
    private GameObject reportCardInstance; 
    private NewDayReportCard reportCard;
    private int currentReportIndex;

    public GameObject missionsPanel;
    public bool HasBeenViewed { get; private set; }

    public ArchivedMission CurrentMissionToReport 
    {
        get => ArchivedMissionsManager.Instance.MissionsCompletedYesterday[currentReportIndex];
    }

    private void Start()
    {
        HasBeenViewed = true;
        reportCardInstance = Instantiate(reportCardPrefab, transform);
        reportCard = reportCardInstance.GetComponent<NewDayReportCard>();
        currentReportIndex = 0;
        ShowNextReport();
    }

    private void ShowNextReport()
    {
        if (ArchivedMissionsManager.WereMissionsCompletedYesterday())
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
        Destroy(reportCardInstance);
        gameObject.SetActive(false);
        missionsPanel.SetActive(true);
    }
}
