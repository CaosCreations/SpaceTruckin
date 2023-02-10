using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RepairsUI : SubMenu, IRepairsUI
{
    [SerializeField] private RepairToolsUI repairToolsUI;
    [SerializeField] private Text feedbackText;
    [SerializeField] private Button repairsMinigameButton;

    [SerializeField] private ResourceBar hullResourceBar;
    [SerializeField] private ShipDetails shipDetails;

    public void SetUp(Ship shipToRepair, RepairsMinigameType minigameType)
    {
        if (shipToRepair != null)
        {
            shipDetails.Init(shipToRepair);
            feedbackText.Clear();
            UpdateHullResourceBar();
            SetButtonInteractability();

            repairsMinigameButton.SetText(RepairsConstants.StartButtonText);
        }
    }

    public void UpdateUI(bool wasSuccessful)
    {
        repairToolsUI.UpdateToolsText();
        UpdateFeedbackText(wasSuccessful);
        SetButtonInteractability();
        UpdateHullResourceBar();
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

    private void UpdateHullResourceBar()
    {
        hullResourceBar.SetResourceValue(ShipsManager.ShipUnderRepair.HullPercentage);
    }

    private void SetButtonInteractability()
    {
        repairsMinigameButton.interactable = ShipsManager.CanRepair;
    }
}
