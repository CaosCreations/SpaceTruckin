using UnityEngine;
using UnityEngine.UI;

public class TerminalUIManager : MonoBehaviour
{
    public Button missionsButton;
    public Button messagesButton;
    public Button analyticsButton;
    public Button crewButton;
    public Button upgradesButton;
    public Text moneyText;

    public GameObject missionsPanel;
    public GameObject messagesPanel;
    public GameObject analyticsPanel;
    public GameObject crewPanel;
    public GameObject upgradesPanel;
    public GameObject newDayReportPanel; 
    private NewDayReportUI newDayReportUI;
    
    public enum Tab
    {
        Missions, Messages, Analytics, Fleet, Licences
    }

    private void Start()
    {
        SetupButtonListeners();
        TabButtonClicked(Tab.Missions);
        newDayReportUI = newDayReportPanel.GetComponent<NewDayReportUI>();
    }

    private void OnEnable()
    {
        if (ArchivedMissionsManager.WereMissionsCompletedYesterday()
            && !newDayReportUI.HasBeenViewed)
        {
            ClearPanels();
            ResetTabButtonColours();
            newDayReportPanel.SetActive(true);
            newDayReportUI.Init();
        }
    }

    private void SetupButtonListeners()
    {
        missionsButton.AddOnClick(() => TabButtonClicked(Tab.Missions));
        messagesButton.AddOnClick(() => TabButtonClicked(Tab.Messages));
        analyticsButton.AddOnClick(() => TabButtonClicked(Tab.Analytics));
        crewButton.AddOnClick(() => TabButtonClicked(Tab.Fleet));
        upgradesButton.AddOnClick(() => TabButtonClicked(Tab.Licences));
    }

    private void TabButtonClicked(Tab tabClicked)
    {
        SetActivePanel(tabClicked);
        SetTabButtonColours(tabClicked);
    }

    private void SetActivePanel(Tab tabClicked)
    {
        ClearPanels();
        GetPanelByTabClicked(tabClicked).SetActive(true);
    }

    private void ClearPanels()
    {
        missionsPanel.SetActive(false);
        messagesPanel.SetActive(false);
        analyticsPanel.SetActive(false);
        crewPanel.SetActive(false);
        upgradesPanel.SetActive(false);
        newDayReportPanel.SetActive(false);
    }

    private GameObject GetPanelByTabClicked(Tab tabClicked)
    {
        switch (tabClicked)
        {
            case Tab.Missions:
                return missionsPanel;
            case Tab.Messages:
                return messagesPanel;
            case Tab.Analytics:
                return analyticsPanel;
            case Tab.Fleet:
                return crewPanel;
            case Tab.Licences:
                return upgradesPanel;
            default:
                return null;
        }
    }

    private void SetTabButtonColours(Tab tabClicked)
    {
        ResetTabButtonColours();
        Color tabButtonColour = GetPanelByTabClicked(tabClicked).GetImageColour();
        GetTabButtonByTabClicked(tabClicked).SetColour(tabButtonColour);
    }

    public void ResetTabButtonColours()
    {
        missionsButton.SetColour(UIConstants.InactiveTabButtonColour);
        messagesButton.SetColour(UIConstants.InactiveTabButtonColour);
        analyticsButton.SetColour(UIConstants.InactiveTabButtonColour);
        crewButton.SetColour(UIConstants.InactiveTabButtonColour);
        upgradesButton.SetColour(UIConstants.InactiveTabButtonColour);
    }

    private Button GetTabButtonByTabClicked(Tab tabClicked)
    {
        switch (tabClicked)
        {
            case Tab.Missions:
                return missionsButton;
            case Tab.Messages:
                return messagesButton;
            case Tab.Analytics:
                return analyticsButton;
            case Tab.Fleet:
                return crewButton;
            case Tab.Licences:
                return upgradesButton;
            default:
                return null;
        }
    }
}
