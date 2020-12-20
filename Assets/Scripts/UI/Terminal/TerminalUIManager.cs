using System.Collections;
using System.Collections.Generic;
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

    public enum Tab
    {
        Missions, Messages, Analytics, Crew, Upgrades
    }

    void Start()
    {
        SetupButtonListeners();
        TabButtonClicked(Tab.Missions);
        UIManager.onCanvasActivated += UpdateMoneyText;
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
    }

    private void ClearPanels()
    {
        missionsPanel.SetActive(false);
        messagesPanel.SetActive(false);
        analyticsPanel.SetActive(false);
        crewPanel.SetActive(false);
        upgradesPanel.SetActive(false);
    }

    // Use with generic event 
    private void UpdateMoneyText(UICanvasType canvasType)
    {
        if (canvasType.Equals(UICanvasType.Terminal))
        {
            moneyText.text = "$ " + PlayerManager.Instance.playerData.playerMoney;
        }
    }

    // Use with terminal-specific event 
    private void UpdateMoneyText()
    {
        if (UIManager.Instance.interactableType.Equals(UICanvasType.Terminal))
        {
            moneyText.text = "$ " + PlayerManager.Instance.playerData.playerMoney;
        }
    }
}
