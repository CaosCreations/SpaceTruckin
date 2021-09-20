﻿using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RepairsUI : SubMenu
{
    [SerializeField] private readonly RepairToolsUI repairToolsUI;
    [SerializeField] private readonly Text feedbackText;
    [SerializeField] private readonly Button repairsMinigameButton;

    [SerializeField] private readonly ResourceBar hullResourceBar;
    [SerializeField] private readonly ShipDetails shipDetails;

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
            // Damage type affects which minigame to play (which part of the ship to repair)
            //RepairsMinigame minigameType = RepairsMinigamesManager
            //    .GetMinigameTypeByDamageType(ShipsManager.ShipUnderRepair.DamageType);

            // Todo: Don't hard-code this once we have multiple minigames
            RepairsMinigame minigameType = RepairsMinigame.Wheel;

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
}
