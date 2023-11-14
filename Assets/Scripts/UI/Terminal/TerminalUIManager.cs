using UnityEngine;
using UnityEngine.UI;

public class TerminalUIManager : UICanvasBase
{
    // Buttons 
    [SerializeField] private Button missionsButton;
    [SerializeField] private Button messagesButton;
    [SerializeField] private Button analyticsButton;
    [SerializeField] private Button fleetButton;
    [SerializeField] private Button licencesButton;
    [SerializeField] private Text moneyText;

    // Panels 
    [SerializeField] private GameObject missionsPanel;
    [SerializeField] private GameObject messagesPanel;
    [SerializeField] private GameObject analyticsPanel;
    [SerializeField] private GameObject fleetPanel;
    [SerializeField] private GameObject licencesPanel;
    [SerializeField] private GameObject newDayReportPanel;

    private NewDayReportUI newDayReportUI;

    public enum Tab
    {
        Missions, Messages, Analytics, Fleet, Licences
    }

    private static Tab currentTab;

    private void Awake()
    {
        SetupButtonListeners();
        TabButtonClicked(Tab.Missions);
        newDayReportUI = newDayReportPanel.GetComponent<NewDayReportUI>();
    }

    private void OnEnable()
    {
        if (ArchivedMissionsManager.ThereAreMissionsToReport()
            && !newDayReportUI.HasBeenViewedToday)
        {
            ClearPanels();
            ResetTabButtonColours();
            newDayReportPanel.SetActive(true);
            newDayReportUI.Init();
        }
    }

    private void SetupButtonListeners()
    {
        missionsButton.AddOnClick(() => TabButtonClicked(Tab.Missions), removeListeners: false);
        messagesButton.AddOnClick(() => TabButtonClicked(Tab.Messages), removeListeners: false);
        analyticsButton.AddOnClick(() => TabButtonClicked(Tab.Analytics), removeListeners: false);
        fleetButton.AddOnClick(() => TabButtonClicked(Tab.Fleet), removeListeners: false);
        licencesButton.AddOnClick(() => TabButtonClicked(Tab.Licences), removeListeners: false);
    }

    private void TabButtonClicked(Tab tab)
    {
        SwitchPanel(tab);
    }

    public void SwitchPanel(Tab tab)
    {
        if (tab == currentTab)
        {
            return;
        }
        ClearPanels();
        GetPanelByTab(tab).SetActive(true);
        SetTabButtonColours(tab);
        currentTab = tab;
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

    private GameObject GetPanelByTab(Tab tab)
    {
        return tab switch
        {
            Tab.Missions => missionsPanel,
            Tab.Messages => messagesPanel,
            Tab.Analytics => analyticsPanel,
            Tab.Fleet => fleetPanel,
            Tab.Licences => licencesPanel,
            _ => null,
        };
    }

    private void SetTabButtonColours(Tab tab)
    {
        ResetTabButtonColours();
        Color tabButtonColour = GetPanelByTab(tab).GetImageColour();
        GetTabButtonByTab(tab).SetColour(tabButtonColour);
    }

    public void ResetTabButtonColours()
    {
        missionsButton.SetColour(UIConstants.InactiveTabButtonColour);
        messagesButton.SetColour(UIConstants.InactiveTabButtonColour);
        analyticsButton.SetColour(UIConstants.InactiveTabButtonColour);
        fleetButton.SetColour(UIConstants.InactiveTabButtonColour);
        licencesButton.SetColour(UIConstants.InactiveTabButtonColour);
    }

    private Button GetTabButtonByTab(Tab tab)
    {
        return tab switch
        {
            Tab.Missions => missionsButton,
            Tab.Messages => messagesButton,
            Tab.Analytics => analyticsButton,
            Tab.Fleet => fleetButton,
            Tab.Licences => licencesButton,
            _ => null,
        };
    }

    public void SetTabButtonsInteractable(bool interactable)
    {
        missionsButton.interactable = interactable;
        messagesButton.interactable = interactable;
        analyticsButton.interactable = interactable;
        fleetButton.interactable = interactable;
        licencesButton.interactable = interactable;
    }

    public void SetSingleTabButtonInteractable(Tab tab)
    {
        SetTabButtonsInteractable(false);
        var tabButton = GetTabButtonByTab(tab);
        tabButton.interactable = true;
    }
}
