﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class HangarNodeUI : UICanvasBase
{
    public enum HangarPanel
    {
        Main, Repair, Upgrade, Customization
    }

    [Header("Set In Editor")]
    public GameObject mainPanel;
    public GameObject repairPanel;
    public RepairsUI repairsUI;
    public GameObject upgradePanel;
    public GameObject customizationPanel;

    public Slider fuelSlider;
    public FuelButton fuelButton;

    public Slider hullSlider;
    public Button hullButton;

    public Button upgradeButton;
    public Button startMissionButton;
    public Button returnToQueueButton;
    public Button customizationButton;

    public Image batteryChargeImage;

    [Header("Set at Runtime")]
    public GameObject shipPreview;
    public int hangarNode;
    public Ship shipToInspect;
    private HangarSlot hangarSlot;

    private bool isInSubMenu;
    private readonly long fuelCostPerUnit = 1;
    private long fuelCostAfterLicences;
    private float fuelTimer = 0;
    private readonly float fuelTimerInterval = 0.025f;
    private bool ThisNodeIsEmpty => shipToInspect == null || shipToInspect.IsLaunched;

    private void OnEnable()
    {
        hangarNode = UIManager.hangarNode;
        hangarSlot = HangarManager.GetSlotByNode(hangarNode);
        shipToInspect = hangarSlot.Ship;

        // There is no ship at this node, don't open UI
        if (ThisNodeIsEmpty)
        {
            UIManager.ClearCanvases();
            return;
        }

        PopulateUI();
        SetButtonInteractability();
        SetBatteryChargeImage();

        fuelCostAfterLicences = GetFuelCostAfterLicences();
        Debug.Log("Fuel cost per unit after licence effect: " + fuelCostAfterLicences);
    }

    private void OnDisable()
    {
        Destroy(shipPreview);
        
        if (ThisNodeIsEmpty)
        {
            UIManager.SetCannotInteract();
        }
    }

    private void Update()
    {
        if (isInSubMenu)
        {
            if (Input.GetKeyDown(PlayerConstants.ExitKey))
            {
                SwitchPanel(HangarPanel.Main);
            }
        }
        else
        {
            if (shipPreview != null)
            {
                shipPreview.transform.Rotate(UIConstants.ShipPreviewRotationSpeed);
            }
        }

        CheckFueling();
    }

    private void PopulateUI()
    {
        SwitchPanel(HangarPanel.Main);
        SetupShipPreview();
        SetSliderValues();

        hullButton.AddOnClick(() => SwitchPanel(HangarPanel.Repair));
        upgradeButton.AddOnClick(() => SwitchPanel(HangarPanel.Upgrade));
        customizationButton.AddOnClick(() => SwitchPanel(HangarPanel.Customization));
        startMissionButton.AddOnClick(() => Launch(isStartingMission: true));
        returnToQueueButton.AddOnClick(() => Launch(isStartingMission: false));
    }

    private void SetupShipPreview()
    {
        shipPreview = Instantiate(shipToInspect.ShipPrefab, transform);
        shipPreview.transform.localScale *= UIConstants.ShipPreviewScaleFactor;
        shipPreview.transform.position += UIConstants.ShipPreviewOffset;
        shipPreview.SetLayerRecursively(UIConstants.ShipPreviewLayer);
    }

    private void CheckFueling()
    {
        fuelTimer += Time.deltaTime;

        if (fuelButton.IsFueling
            && fuelTimer > fuelTimerInterval
            && shipToInspect.CurrentFuel < shipToInspect.MaxFuel
            && PlayerManager.Instance.CanSpendMoney(fuelCostPerUnit)
            )
        {
            PlayerManager.Instance.SpendMoney(fuelCostAfterLicences);
            shipToInspect.CurrentFuel++;
            fuelSlider.value = shipToInspect.GetFuelPercent();
            fuelTimer = 0;
            SetButtonInteractability();
        }
    }

    private void SwitchPanel(HangarPanel panel)
    {
        mainPanel.SetActive(false);
        repairPanel.SetActive(false);
        upgradePanel.SetActive(false);
        customizationPanel.SetActive(false);

        switch (panel)
        {
            case HangarPanel.Main:
                mainPanel.SetActive(true);
                SetSliderValues();
                SetButtonInteractability();
                break;
            case HangarPanel.Repair:
                repairPanel.SetActive(true);
                repairsUI.Init(shipToInspect);
                break;
            case HangarPanel.Upgrade:
                upgradePanel.SetActive(true);
                break;
            case HangarPanel.Customization:
                customizationPanel.SetActive(true);
                break;
        }

        isInSubMenu = !(panel == HangarPanel.Main);
        UIManager.Instance.currentMenuOverridesEscape = isInSubMenu;
    }

    private void Launch(bool isStartingMission)
    {
        if (shipToInspect != null)
        {
            // Launch ship regardless of mission status 
            HangarManager.LaunchShip(hangarNode);

            ScheduledMission scheduled = MissionsManager.GetScheduledMission(shipToInspect);
            if (scheduled != null)
            {
                // If starting a mission, start it. Otherwise unschedule and return ship to queue. 
                if (isStartingMission)
                {
                    scheduled.Mission.StartMission();
                    Debug.Log($"{scheduled.Pilot.Name} (Pilot) has started {scheduled.Mission.Name} (Mission)");
                }
                else
                {
                    MissionsManager.RemoveScheduledMission(scheduled);
                }
            }

            shipToInspect = null;
            UIManager.ClearCanvases();
        }
        else
        {
            Debug.Log($"{shipToInspect} (Ship) has no fuel!");
        }
    }

    private void SetLayerRecursively(GameObject gameObject, int newLayer)
    {
        gameObject.layer = newLayer;

        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    private bool FuelButtonIsInteractable()
    {
        return shipToInspect.CurrentFuel < shipToInspect.MaxFuel
            && PlayerManager.Instance.CanSpendMoney(fuelCostPerUnit);
    }

    private bool StartMissionButtonIsInteractable()
    {
        if (shipToInspect.CurrentMission != null)
        {
            return shipToInspect.CurrentFuel >= shipToInspect.CurrentMission.FuelCost
                && shipToInspect.CurrentHullIntegrity > 0
                && shipToInspect.CanWarp;
        }
        return false;
    }

    private long GetFuelCostAfterLicences()
    {
        return Convert.ToInt64(fuelCostPerUnit * (1 - LicencesManager.FuelDiscountEffect));
    }

    private void SetBatteryChargeImage()
    {
        batteryChargeImage.color = shipToInspect.CanWarp ?
            HangarConstants.ChargedBatteryImageColour :
            HangarConstants.DepletedBatteryImageColour;
    }

    private void SetSliderValues()
    {
        fuelSlider.value = shipToInspect.GetFuelPercent();
        hullSlider.value = shipToInspect.GetHullPercent();
    }

    private void SetButtonInteractability()
    {
        fuelButton.Button.interactable = FuelButtonIsInteractable();
        startMissionButton.interactable = StartMissionButtonIsInteractable();
        hullButton.interactable = !shipToInspect.IsFullyRepaired;
    }

    public override void ShowTutorial()
    {
        if (CanvasTutorialPrefab != null)
        {
            GameObject tutorial = Instantiate(CanvasTutorialPrefab, transform);

            CardCycle cardCycle = tutorial.GetComponent<CardCycle>();

            if (cardCycle != null)
            {
                // Show the name of the currently docked ship in the tutorial cards
                cardCycle.CyclableContent.ReplaceTemplates(shipToInspect);

                cardCycle.SetupCardCycle();
            }
        }
    }
}
