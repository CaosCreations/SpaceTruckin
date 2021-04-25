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
    public bool HasBeenViewed { get; set; }
    public ArchivedMission CurrentMissionToReport 
    {
        get => ArchivedMissionsManager.Instance.MissionsCompletedYesterday[currentReportIndex];
    }

    private void Awake()
    {
        BedCanvasUI.OnEndOfDay += () => HasBeenViewed = false;
        terminalManager = GetComponentInParent<TerminalUIManager>();
        nextCardButton = GetComponentInChildren<Button>(includeInactive: true);
    }
    
    private void OnDisable() => HasBeenViewed = true;

    public void Init()
    {
        reportCardInstance.SetActive(true);
        currentReportIndex = 0;
        nextCardButton.AddOnClick(ShowNextReport);
        nextCardButton.onClick.Invoke();
        welcomeMessageText.ReplaceTemplates();
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
            }
            else
            {
                reportCard.nextCardButton.AddOnClick(CloseReport).SetText(UIConstants.CloseReportButtonText);
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
        if (reportCardInstance != null && Input.GetKeyDown(PlayerConstants.ExitKey))
        {
            CloseReport();
        }
    }
}
