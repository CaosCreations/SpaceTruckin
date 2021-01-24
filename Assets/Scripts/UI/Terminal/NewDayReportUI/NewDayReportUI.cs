using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewDayReportUI : MonoBehaviour
{
    public GameObject reportCardPrefab;
    private GameObject reportCardInstance; 
    private NewDayReportCard reportCard;
    private int currentReportIndex;

    public GameObject missionsPanel;

    public Mission CurrentMissionToReport 
    {
        get => MissionsManager.Instance.MissionsCompletedYesterday[currentReportIndex];
    }

    private void Start()
    {
        reportCardInstance = Instantiate(reportCardPrefab, transform);
        reportCard = reportCardInstance.GetComponent<NewDayReportCard>();
        currentReportIndex = 0;
        ShowNextReport();
    }

    private void ShowNextReport()
    {
        if (MissionsManager.Instance.MissionsCompletedYesterday != null 
            && MissionsManager.Instance.MissionsCompletedYesterday.Any())
        {
            reportCard.ShowReport(CurrentMissionToReport);

            if (currentReportIndex < MissionsManager.Instance.MissionsCompletedYesterday.Count - 1)
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
