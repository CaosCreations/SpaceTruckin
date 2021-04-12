using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RepairsUI : MonoBehaviour
{
    [SerializeField] private RepairToolsUI repairToolsUI;
    [SerializeField] private Text feedbackText;
    [SerializeField] private Button stopStartButton;
    [SerializeField] private ResourceBar hullResourceBar;
    [SerializeField] private ShipDetails shipDetails;

    [SerializeField] private GameObject repairsMinigamePrefab;
    private GameObject repairsMinigameInstance;
    private RepairsManager repairsManager;

    public Ship ShipToRepair { get; set; }

    private void Start()
    {
        stopStartButton.AddOnClick(HandleStopStart);
    }

    public void Init(Ship shipToRepair)
    {
        if (shipToRepair != null)
        {
            ShipToRepair = shipToRepair;
            shipDetails.Init(shipToRepair);
            UpdateHullResourceBar();
            feedbackText.Clear();
            SetupMinigame();
        }
    }
    
    public void UpdateUI(bool wasSuccessful)
    {
        repairToolsUI.UpdateToolsText();
        UpdateFeedbackText(wasSuccessful);
    }

    private void UpdateFeedbackText(bool wasSuccessful)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(
            wasSuccessful ? RepairsConstants.SuccessMessage : RepairsConstants.FailureMessage);
        
        builder.AppendLine($"You have {PlayerManager.Instance.RepairTools} tools remaining.");

        feedbackText.SetText(builder.ToString());
    }

    public void ResetFeedbackText()
    {
        feedbackText.Clear();
    }

    public void SetupMinigame()
    {
        if (repairsMinigameInstance == null)
        {
            repairsMinigameInstance = Instantiate(repairsMinigamePrefab, transform);
            repairsMinigameInstance.SetLayerRecursively(UIConstants.RepairsMinigameLayer);
            repairsManager = repairsMinigameInstance.GetComponent<RepairsManager>();
        }

        stopStartButton.SetText(RepairsConstants.StartButtonText);
        stopStartButton.interactable = PlayerManager.CanRepair;
    }

    private void HandleStopStart()
    {
        repairsManager.StopStart();

        if (repairsManager.IsRepairing)
        {
            stopStartButton.SetText(RepairsConstants.StopButtonText);
            feedbackText.Clear();
        }
        else
        {
            stopStartButton.SetText(RepairsConstants.StartButtonText);
            stopStartButton.interactable = PlayerManager.CanRepair;
            UpdateHullResourceBar();
        }
    }

    private void UpdateHullResourceBar()
    {
        float hullPercentage = ShipToRepair.GetHullPercent();
        hullResourceBar.SetResourceValue(hullPercentage);
    }
}