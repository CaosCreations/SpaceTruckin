﻿using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RepairsUI : SubMenu
{
    [SerializeField] private RepairToolsUI repairToolsUI;
    [SerializeField] private Text feedbackText;
    [SerializeField] private Button stopStartButton;
    [SerializeField] private ResourceBar hullResourceBar;
    [SerializeField] private ShipDetails shipDetails;

    [SerializeField] private GameObject repairsMinigamePrefab;
    private GameObject repairsMinigameInstance;
    private GameObject repairsMinigameUIInstance;
    private WheelManager repairsManager;

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
            feedbackText.Clear();
            stopStartButton.SetText(RepairsConstants.StartButtonText);
            UpdateHullResourceBar();
            SetButtonInteractability();

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
            MinigamePrefab prefabType = GetMinigameTypeByDamageType(ShipDamageType.Engine);
            repairsMinigameInstance = MinigamePrefabManager.Instance.InitPrefab(prefabType, transform);
            repairsMinigameUIInstance = MinigamePrefabManager.Instance.InitUIPrefab(prefabType, transform);

            repairsMinigameInstance.SetLayerRecursively(UIConstants.RepairsMinigameLayer);

            repairsManager = repairsMinigameInstance.GetComponent<WheelManager>();
        }
    }

    private void UpdateHullResourceBar()
    {
        float hullPercentage = ShipToRepair.GetHullPercent();
        hullResourceBar.SetResourceValue(hullPercentage);
    }

    private void SetButtonInteractability()
    {
        stopStartButton.interactable = PlayerManager.CanRepair
            && !ShipToRepair.IsFullyRepaired;
    }

    private MinigamePrefab GetMinigameTypeByDamageType(ShipDamageType damageType)
    {
        return damageType switch
        {
            ShipDamageType.Engine => MinigamePrefab.Wheel,
            ShipDamageType.Hull => MinigamePrefab.Stack,
            _ => throw new ArgumentOutOfRangeException(),
        };

        // Todo: Better default handling here
    }
}
