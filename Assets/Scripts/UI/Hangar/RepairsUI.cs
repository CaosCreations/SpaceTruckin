using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RepairsUI : SubMenu
{
    [SerializeField] private RepairToolsUI repairToolsUI;
    [SerializeField] private Text feedbackText;
    [SerializeField] private Button repairsMinigameButton;

    [SerializeField] private ResourceBar hullResourceBar;
    [SerializeField] private ShipDetails shipDetails;

    private GameObject repairsMinigameInstance;

    private void Awake()
    {
        RepairsMinigamesManager.OnMinigameAttemptFinished += UpdateUI;
    }

    public void Init(Ship shipToRepair)
    {
        if (shipToRepair != null)
        {
            shipDetails.Init(shipToRepair);
            feedbackText.Clear();
            UpdateHullResourceBar();
            SetButtonInteractability();

            repairsMinigameButton.SetText(RepairsConstants.StartButtonText);
            InitMinigame();
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

    public void InitMinigame()
    {
        if (repairsMinigameInstance == null)
        {
            RepairsMinigame minigameType = GetMinigameTypeByDamageType(ShipDamageType.Engine);
            repairsMinigameInstance = MinigamePrefabManager.Instance.InitPrefab(minigameType, transform);

            RepairsMinigamesManager.SetButtonVisibility(minigameType);
            repairsMinigameInstance.SetLayerRecursively(UIConstants.RepairsMinigameLayer);
        }
    }

    private void UpdateHullResourceBar()
    {
        hullResourceBar.SetResourceValue(ShipsManager.ShipUnderRepair.HullPercentage);
    }

    private void SetButtonInteractability()
    {
        repairsMinigameButton.interactable = ShipsManager.CanRepair;
    }

    private RepairsMinigame GetMinigameTypeByDamageType(ShipDamageType damageType)
    {
        return damageType switch
        {
            ShipDamageType.Engine => RepairsMinigame.Wheel,
            ShipDamageType.Hull => RepairsMinigame.Stack,
            _ => throw new ArgumentOutOfRangeException(),
        };

        // Todo: Better default handling here
    }
}
