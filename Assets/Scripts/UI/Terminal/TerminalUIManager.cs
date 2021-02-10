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
        Missions, Messages, Analytics, Crew, Upgrades
    }

    private void Start()
    {
        SetupButtonListeners();
        TabButtonClicked(Tab.Missions);
        newDayReportUI = newDayReportPanel.GetComponent<NewDayReportUI>();

        UpdateMoneyText();
        PlayerManager.onFinancialTransaction += UpdateMoneyText;
    }

    private void OnEnable()
    {
        if (ArchivedMissionsManager.WereMissionsCompletedYesterday() 
            && !newDayReportUI.HasBeenViewed)
        {
            newDayReportPanel.SetActive(true);
            newDayReportUI.Init();
            ClearPanels();
            ResetTabButtonColours();
        }
    }

    private void SetupButtonListeners()
    {
        missionsButton.AddOnClick(() => TabButtonClicked(Tab.Missions));
        messagesButton.AddOnClick(() => TabButtonClicked(Tab.Messages));
        analyticsButton.AddOnClick(() => TabButtonClicked(Tab.Analytics));
        crewButton.AddOnClick(() => TabButtonClicked(Tab.Crew));
        upgradesButton.AddOnClick(() => TabButtonClicked(Tab.Upgrades));
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
            case Tab.Crew:
                return crewPanel;
            case Tab.Upgrades:
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
            case Tab.Crew:
                return crewButton;
            case Tab.Upgrades:
                return upgradesButton;
            default:
                return null;
        }
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "$ " + PlayerManager.Instance.Money;
    }
}
