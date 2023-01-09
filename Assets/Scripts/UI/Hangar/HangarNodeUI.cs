using System;
using UnityEngine;
using UnityEngine.UI;

public class HangarNodeUI : UICanvasBase
{
    public enum HangarPanel
    {
        Main, Repair, Upgrade, Customization
    }

    [Header("Set In Editor")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject repairPanel;
    [SerializeField] private IsolatedRepairsUI repairsUI;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject customizationPanel;

    [SerializeField] private Slider fuelSlider;
    [SerializeField] private FuelButton fuelButton;

    [SerializeField] private Slider hullSlider;
    [SerializeField] private Button hullButton;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button startMissionButton;
    [SerializeField] private Button returnToQueueButton;
    [SerializeField] private Button customizationButton;

    [SerializeField] private Image batteryChargeImage;

    [Header("Set at Runtime")]
    [SerializeField] private GameObject shipPreview;
    [SerializeField] private int hangarNode;
    public Ship ShipToInspect;
    private HangarSlot hangarSlot;

    private readonly long fuelCostPerUnit = 1;
    private long fuelCostAfterLicences;
    private float fuelTimer = 0;
    private readonly float fuelTimerInterval = 0.025f;
    private bool ThisNodeIsEmpty => ShipToInspect == null || ShipToInspect.IsLaunched;

    public static event Action<Ship> OnHangarNodeTerminalOpened;
    public static event Action OnHangarNodeTerminalClosed;

    private void OnEnable()
    {
        hangarNode = UIManager.HangarNode;
        hangarSlot = HangarManager.GetSlotByNode(hangarNode);
        ShipToInspect = hangarSlot.Ship;

        // There is no ship at this node, don't open UI
        if (ThisNodeIsEmpty)
        {
            UIManager.ClearCanvases();
            return;
        }

        SetupUIElements();

        fuelCostAfterLicences = GetFuelCostAfterLicences();
        Debug.Log("Fuel cost per unit after licence effect: " + fuelCostAfterLicences);

        OnHangarNodeTerminalOpened?.Invoke(ShipToInspect);
    }

    private void SetupUIElements()
    {
        PopulateUI();
        SetButtonInteractability();
        SetBatteryChargeImage();
    }

    private void OnDisable()
    {
        Destroy(shipPreview);
        MinigamePrefabManager.DestroyPrefabs();

        if (ThisNodeIsEmpty)
        {
            UIManager.SetCannotInteract();
        }

        OnHangarNodeTerminalClosed?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.ExitKey))
        {
            SwitchPanel(HangarPanel.Main);
        }
        else if (shipPreview != null)
        {
            shipPreview.transform.Rotate(UIConstants.ShipPreviewRotationSpeed);
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
        shipPreview = Instantiate(ShipToInspect.ShipPrefab, transform);
        shipPreview.transform.localScale *= UIConstants.ShipPreviewScaleFactor;
        shipPreview.transform.position += UIConstants.ShipPreviewOffset;
        shipPreview.SetLayerRecursively(UIConstants.ShipPreviewLayer);
    }

    private void CheckFueling()
    {
        fuelTimer += Time.deltaTime;

        if (fuelButton.IsFueling
            && fuelTimer > fuelTimerInterval
            && ShipToInspect.CurrentFuel < ShipToInspect.MaxFuel
            && PlayerManager.Instance.CanSpendMoney(fuelCostPerUnit)
            )
        {
            PlayerManager.Instance.SpendMoney(fuelCostAfterLicences);
            ShipToInspect.CurrentFuel++;
            fuelSlider.value = ShipToInspect.GetFuelPercentage();
            fuelTimer = 0;
            SetButtonInteractability();
        }
    }

    public void SwitchPanel(HangarPanel panel)
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
                repairsUI.Init(ShipToInspect, RepairsMinigameType.Wheel);
                break;
            case HangarPanel.Upgrade:
                upgradePanel.SetActive(true);
                break;
            case HangarPanel.Customization:
                customizationPanel.SetActive(true);
                break;
        }
    }

    private void Launch(bool isStartingMission)
    {
        if (ShipToInspect != null)
        {
            // Launch ship regardless of mission status 
            HangarManager.LaunchShip(hangarNode);

            ScheduledMission scheduled = MissionsManager.GetScheduledMission(ShipToInspect);
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

            ShipToInspect = null;
            UIManager.ClearCanvases();
        }
        else
        {
            Debug.Log($"{ShipToInspect} (Ship) has no fuel!");
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
        return ShipToInspect.CurrentFuel < ShipToInspect.MaxFuel
            && PlayerManager.Instance.CanSpendMoney(fuelCostPerUnit);
    }

    private bool StartMissionButtonIsInteractable()
    {
        if (ShipToInspect.CurrentMission != null)
        {
            return ShipToInspect.CurrentFuel >= ShipToInspect.CurrentMission.FuelCost
                && ShipToInspect.CurrentHullIntegrity > 0
                && ShipToInspect.CanWarp;
        }
        return false;
    }

    private long GetFuelCostAfterLicences()
    {
        return Convert.ToInt64(fuelCostPerUnit * (1 - LicencesManager.FuelDiscountEffect));
    }

    private void SetBatteryChargeImage()
    {
        batteryChargeImage.color = ShipToInspect.CanWarp ?
            HangarConstants.ChargedBatteryImageColour :
            HangarConstants.DepletedBatteryImageColour;
    }

    private void SetSliderValues()
    {
        fuelSlider.value = ShipToInspect.GetFuelPercentage();
        hullSlider.value = ShipToInspect.GetHullPercentage();
    }

    private void SetButtonInteractability()
    {
        fuelButton.Button.interactable = FuelButtonIsInteractable();
        startMissionButton.interactable = StartMissionButtonIsInteractable();
        hullButton.interactable = !ShipToInspect.IsFullyRepaired;
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
                cardCycle.CyclableContent.ReplaceTemplates(ShipToInspect);

                cardCycle.SetupCardCycle();
            }
        }
    }
}
