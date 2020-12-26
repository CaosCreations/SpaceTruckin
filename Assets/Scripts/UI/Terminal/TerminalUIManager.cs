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

    public static event System.Action onTabButtonClicked; 

    public enum Tab
    {
        Missions, Messages, Analytics, Crew, Upgrades
    }

    void Start()
    {
        SetupButtonListeners();
        TabButtonClicked(Tab.Missions);
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

        // We need to close the pilots profile panel if the player navigates away from it 
        // without hitting the back button, as otherwise it will conflict with the other panels.
        if (pilotsUI.pilotProfilePanel != null && pilotsUI.pilotProfilePanel.activeSelf)
        {
            onTabButtonClicked?.Invoke();   
        }
    }

    private void ClearPanels()
    {
        missionsPanel.SetActive(false);
        messagesPanel.SetActive(false);
        analyticsPanel.SetActive(false);
        crewPanel.SetActive(false);
        upgradesPanel.SetActive(false);
    }

    private void UpdateMoneyText(long updatedMoney)
    {
        moneyText.text = "$ " + updatedMoney;
    }
}
