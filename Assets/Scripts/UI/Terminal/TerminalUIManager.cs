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

    public enum Tab
    {
        Missions, Messages, Analytics, Crew, Upgrades
    }

    private void Start()
    {
        SetupButtonListeners();
        TabButtonClicked(Tab.Missions);
        UpdateMoneyText();
        PlayerManager.onFinancialTransaction += UpdateMoneyText;
    }

    private void OnEnable()
    {
        if (MissionsManager.MissionsWereCompletedYesterday())
        {
            newDayReportPanel.SetActive(true);
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
        ClearPanels();
        switch (tabClicked)
        {
            case Tab.Missions:
                missionsPanel.SetActive(true);
                break;
            case Tab.Messages:
                messagesPanel.SetActive(true);
                break;
            case Tab.Analytics:
                analyticsPanel.SetActive(true);
                break;
            case Tab.Crew:
                crewPanel.SetActive(true);
                break;
            case Tab.Upgrades:
                upgradesPanel.SetActive(true);
                break;
        }
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

    private void UpdateMoneyText()
    {
        moneyText.text = "$ " + PlayerManager.Instance.Money;
    }
}
