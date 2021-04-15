using UnityEngine;
using UnityEngine.UI;

public class TerminalUIManager : UICanvasBase
{
    public Button missionsButton;
    public Button messagesButton;
    public Button analyticsButton;
    public Button fleetButton;
    public Button licencesButton;
    public Text moneyText;

    public GameObject missionsPanel;
    public GameObject messagesPanel;
    public GameObject analyticsPanel;
    public GameObject fleetPanel;
    public GameObject licencesPanel;
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
        fleetButton.AddOnClick(() => TabButtonClicked(Tab.Fleet));
        licencesButton.AddOnClick(() => TabButtonClicked(Tab.Licences));
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
        fleetPanel.SetActive(false);
        licencesPanel.SetActive(false);
        newDayReportPanel.SetActive(false);
    }

    private GameObject GetPanelByTabClicked(Tab tabClicked)
    {
        return tabClicked switch
        {
            Tab.Missions => missionsPanel,
            Tab.Messages => messagesPanel,
            Tab.Analytics => analyticsPanel,
            Tab.Fleet => fleetPanel,
            Tab.Licences => licencesPanel,
            _ => null,
        };
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
        fleetButton.SetColour(UIConstants.InactiveTabButtonColour);
        licencesButton.SetColour(UIConstants.InactiveTabButtonColour);
    }

    private Button GetTabButtonByTabClicked(Tab tabClicked)
    {
        return tabClicked switch
        {
            Tab.Missions => missionsButton,
            Tab.Messages => messagesButton,
            Tab.Analytics => analyticsButton,
            Tab.Fleet => fleetButton,
            Tab.Licences => licencesButton,
            _ => null,
        };
    }
}
