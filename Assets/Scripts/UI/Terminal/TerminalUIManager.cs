using System.Reflection;
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
    public PilotsUI pilotsUI;
    public GameObject upgradesPanel;

    public enum Tab
    {
        Missions, Messages, Analytics, Crew, Upgrades
    }

    void Start()
    {
        SetupButtonListeners();
        TabButtonClicked(Tab.Missions);
        UpdateMoneyText();
        PlayerManager.onFinancialTransaction += UpdateMoneyText;
    }

    private void SetupButtonListeners()
    {
        missionsButton.onClick.RemoveAllListeners();
        missionsButton.onClick.AddListener(delegate { TabButtonClicked(Tab.Missions); });

        messagesButton.onClick.RemoveAllListeners();
        messagesButton.onClick.AddListener(delegate { TabButtonClicked(Tab.Messages); });

        analyticsButton.onClick.RemoveAllListeners();
        analyticsButton.onClick.AddListener(delegate { TabButtonClicked(Tab.Analytics); });

        crewButton.onClick.RemoveAllListeners();
        crewButton.onClick.AddListener(delegate { TabButtonClicked(Tab.Crew); });

        upgradesButton.onClick.RemoveAllListeners();
        upgradesButton.onClick.AddListener(delegate { TabButtonClicked(Tab.Upgrades); });
    }

    private void TabButtonClicked(Tab tabClicked)
    {
        ClearPanels();
        GetPanelByTabClicked(tabClicked).SetActive(true);
        SetTabButtonColours(tabClicked);
    }

    private void ClearPanels()
    {
        missionsPanel.SetActive(false);
        messagesPanel.SetActive(false);
        analyticsPanel.SetActive(false);
        crewPanel.SetActive(false);
        upgradesPanel.SetActive(false);
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
        missionsButton.SetColour(UIConstants.InactiveTabButtonColour);
        messagesButton.SetColour(UIConstants.InactiveTabButtonColour);
        analyticsButton.SetColour(UIConstants.InactiveTabButtonColour);
        crewButton.SetColour(UIConstants.InactiveTabButtonColour);
        upgradesButton.SetColour(UIConstants.InactiveTabButtonColour);
        GetTabButtonByTabClicked(tabClicked).SetColour(UIConstants.ActiveTabButtonColour);
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
