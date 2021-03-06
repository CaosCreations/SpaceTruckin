﻿using UnityEngine;
using UnityEngine.UI;

public class HangarNodeUI : MonoBehaviour
{
    public enum HangarPanel
    {
        Main, Repair, Upgrade, Customization
    }

    [Header("Set In Editor")]
    public GameObject mainPanel;
    public GameObject repairPanel;
    public GameObject upgradePanel;
    public GameObject customizationPanel;

    public Slider fuelSlider;
    public FuelButton fuelButton;

    public Slider hullSlider;
    public Button hullButton;

    public Button upgradeButton;
    public Button launchButton;
    public Button customizationButton;

    [Header("Set at Runtime")]
    public GameObject shipPreview;
    public HangarNode hangarNode;
    public Ship shipToInspect;

    private bool isInSubMenu;
    private long fuelCostPerUnit = 1;
    private float fuelTimer = 0;
    private float fuelTimerInterval = 0.025f;

    private void OnEnable()
    {
        hangarNode = UIManager.Instance.hangarNode;
        shipToInspect = ShipsManager.GetShipForNode(hangarNode);

        // There is no ship at this node, don't open UI
        if(shipToInspect == null || shipToInspect.IsLaunched)
        {
            UIManager.ClearCanvases();
            return;
        }

        PopulateUI();
        fuelButton.button.interactable = ShouldFuelButtonBeInteractable();
        launchButton.interactable = ShouldLaunchButtonBeInteractable();
    }

    private void OnDisable()
    {
        Destroy(shipPreview);
    }

    void Update()
    {
        if (isInSubMenu)
        {
            if (Input.GetKeyDown(PlayerConstants.exit))
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

        fuelSlider.value = shipToInspect.GetFuelPercent();

        hullSlider.value = shipToInspect.GetHullPercent();
        hullButton.onClick.RemoveAllListeners();
        hullButton.onClick.AddListener(() => SwitchPanel(HangarPanel.Repair));

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => SwitchPanel(HangarPanel.Upgrade));

        customizationButton.onClick.RemoveAllListeners();
        customizationButton.onClick.AddListener(() => SwitchPanel(HangarPanel.Customization));

        launchButton.onClick.RemoveAllListeners();
        launchButton.onClick.AddListener(Launch);
    }

    private void SetupShipPreview()
    {
        shipPreview = Instantiate(shipToInspect.ShipPrefab, transform);
        shipPreview.transform.localScale *= UIConstants.ShipPreviewScaleFactor;
        shipPreview.transform.position += UIConstants.ShipPreviewOffset;
        SetLayerRecursively(shipPreview, UIConstants.UIObjectLayer);
    }

    private void CheckFueling()
    {
        fuelTimer += Time.deltaTime;

        if (fuelButton.isFueling
            && fuelTimer > fuelTimerInterval
            && shipToInspect.CurrentFuel < shipToInspect.MaxFuel
            && PlayerManager.Instance.CanSpendMoney(fuelCostPerUnit)
            )
        {
            PlayerManager.Instance.SpendMoney(fuelCostPerUnit);
            shipToInspect.CurrentFuel++;
            fuelSlider.value = shipToInspect.GetFuelPercent();
            fuelTimer = 0;
            fuelButton.button.interactable = ShouldFuelButtonBeInteractable();
            launchButton.interactable = ShouldLaunchButtonBeInteractable();
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
                break;
            case HangarPanel.Repair:
                repairPanel.SetActive(true);
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

    private void Launch()
    {
        if (shipToInspect.CurrentFuel > 0
            && shipToInspect.CurrentMission != null)
        {
            ShipsManager.LaunchShip(hangarNode);
            UIManager.ClearCanvases();
        }
        else
        {
            Debug.Log("Ship has no fuel!");
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

    private bool ShouldFuelButtonBeInteractable()
    {
        return shipToInspect.CurrentFuel < shipToInspect.MaxFuel
            && PlayerManager.Instance.CanSpendMoney(fuelCostPerUnit);
    }

    private bool ShouldLaunchButtonBeInteractable()
    {
        if (shipToInspect.CurrentMission != null)
        {
            return shipToInspect.CurrentFuel >= shipToInspect.CurrentMission.FuelCost
                && shipToInspect.CurrentHullIntegrity > 0;
        }
        return false;
    }
}
