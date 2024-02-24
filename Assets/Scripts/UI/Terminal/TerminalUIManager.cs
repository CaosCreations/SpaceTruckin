using System.Collections.Generic;
using System.Linq;
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

    // Tab colours
    [SerializeField] private Color missionsTabColour;
    [SerializeField] private Color messagesTabColour;
    [SerializeField] private Color analyticsTabColour;
    [SerializeField] private Color fleetTabColour;
    [SerializeField] private Color licencesTabColour;
    private Dictionary<Tab, Color> tabColourMap;
    //private Color unclickedButtonText = new(50, 50, 50);
    //private Color clickedButtonText = new(242, 240, 229);

    private NewDayReportUI newDayReportUI;

    public enum Tab
    {
        Missions, Messages, Analytics, Fleet, Licences
    }

    [SerializeField] private Tab[] demoFeatureTabs;
    private static Tab currentTab;

    private void Awake()
    {
        SetupButtonListeners();
        TabButtonClicked(Tab.Missions);
        newDayReportUI = newDayReportPanel.GetComponent<NewDayReportUI>();

        tabColourMap = new()
        {
            { Tab.Missions, missionsTabColour },
            { Tab.Messages, messagesTabColour },
            { Tab.Analytics, analyticsTabColour },
            { Tab.Fleet, fleetTabColour },
            { Tab.Licences, licencesTabColour },
        };
    }

    private void OnEnable()
    {
        if (ArchivedMissionsManager.ThereAreMissionsToReport() && !newDayReportUI.HasBeenViewedToday)
        {
            ClearPanels();
            ResetTabButtonColours();
            newDayReportPanel.SetActive(true);
            newDayReportUI.Init();
        }
        else
        {
            SwitchPanel(Tab.Missions, true);
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

    public void SwitchPanel(Tab tab, bool reset = false)
    {
        if (!reset && tab == currentTab)
        {
            return;
        }
        ClearPanels();
        GetPanelByTab(tab).SetActive(true);
        SetTabButtonColours(tab);
        currentTab = tab;

        if (demoFeatureTabs.Contains(currentTab))
        {
            PopupManager.ShowPopup(type: PopupType.DemoFeature);
        }
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
        var tabButtonColour = tabColourMap[tab];
        var button = GetTabButtonByTab(tab);
        //button.SetTextColour(clickedButtonText);
        button.SetColour(tabButtonColour);
    }

    public void ResetTabButtonColours()
    {
        missionsButton.SetColour(UIConstants.InactiveTabButtonColour);
        messagesButton.SetColour(UIConstants.InactiveTabButtonColour);
        analyticsButton.SetColour(UIConstants.InactiveTabButtonColour);
        fleetButton.SetColour(UIConstants.InactiveTabButtonColour);
        licencesButton.SetColour(UIConstants.InactiveTabButtonColour);

        //missionsButton.SetTextColour(unclickedButtonText);
        //messagesButton.SetTextColour(unclickedButtonText);
        //analyticsButton.SetTextColour(unclickedButtonText);
        //fleetButton.SetTextColour(unclickedButtonText);
        //licencesButton.SetTextColour(unclickedButtonText);
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
