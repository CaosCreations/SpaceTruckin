using Events;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UniversalHangarNodeUI : UICanvasBase
{
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private FuelButton fuelButton;

    [SerializeField] private Slider hullSlider;
    [SerializeField] private Button repairsButton;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button startMissionButton;
    [SerializeField] private Button returnToQueueButton;
    [SerializeField] private Button customizationButton;
    [SerializeField] private Button overviewButton;

    [SerializeField] private ImageAnimator batteryChargeAnimator;

    [Header("Set at Runtime")]
    [SerializeField] private GameObject shipPreview;
    [SerializeField] private HangarNodePilotDetails hangarNodePilotDetails;
    public Ship ShipToInspect;
    private HangarSlot hangarSlot;

    private readonly long fuelCostPerUnit = 1;
    private long fuelCostAfterLicences;
    private float fuelTimer = 0;
    private readonly float fuelTimerInterval = 0.025f;
    private bool ThisNodeIsEmpty => ShipToInspect == null || ShipToInspect.IsLaunched;

    private void Awake()
    {
        repairsButton.AddOnClick(RepairsButtonHandler);
        overviewButton.AddOnClick(OverviewButtonHandler);
        SingletonManager.EventService.Add<OnShipHealthChangedEvent>(OnShipHealthChangedHandler);
    }

    private void RepairsButtonHandler()
    {
        //shipPreview.SetActive(false);
        //SceneRepairsMinigamesManager.Instance.StartMinigame();
        PopupManager.ShowPopup(type: PopupType.DemoFeature);
        ShipsManager.RepairShip(100f);
        SetButtonInteractability();
    }

    private void OverviewButtonHandler()
    {
        //StopMinigame();
    }

    private void StopMinigame()
    {
        shipPreview.SetActive(true);
        SceneRepairsMinigamesManager.Instance.StopMinigame();
    }

    private void OnEnable()
    {
        hangarSlot = HangarManager.GetSlotByNode(UIManager.HangarNode);

        if (hangarSlot != null)
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

        SingletonManager.EventService.Dispatch(new OnHangarNodeTerminalOpenedEvent(ShipToInspect));
    }

    private void SetupUIElements()
    {
        fuelButton.Init(ShipToInspect, fuelCostPerUnit);
        PopulateUI();
        SetButtonInteractability();
        SetBatteryChargeImage();
    }

    private void OnDisable()
    {
        Destroy(shipPreview);

        if (ThisNodeIsEmpty)
        {
            UIManager.SetCannotInteract();
        }

        SceneRepairsMinigamesManager.Instance.StopMinigame();
        SingletonManager.EventService.Dispatch<OnHangarNodeTerminalClosedEvent>();
    }

    private void Update()
    {
        if (shipPreview != null)
        {
            shipPreview.transform.Rotate(UIConstants.ShipPreviewRotationSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneRepairsMinigamesManager.Instance.IsRepairsMinigameRunning())
        {
            StopMinigame();
        }

        CheckFueling();
    }

    private void PopulateUI()
    {
        SetupShipPreview();
        SetupPilotDetails();
        SetSliderValues();

        startMissionButton.AddOnClick(() => Launch(isStartingMission: true), removeListeners: false);
        returnToQueueButton.AddOnClick(() => Launch(isStartingMission: false), removeListeners: false);
    }

    private void SetupPilotDetails()
    {
        Pilot pilot = PilotsManager.GetPilotByShip(ShipToInspect);
        hangarNodePilotDetails.Init(pilot);
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
            && !ShipToInspect.IsFullyFuelled
            && PlayerManager.Instance.CanSpendMoney(fuelCostPerUnit))
        {
            PlayerManager.Instance.SpendMoney(fuelCostAfterLicences);
            ShipToInspect.CurrentFuel++;
            fuelSlider.value = ShipToInspect.GetFuelPercentage();
            fuelTimer = 0;
            SetButtonInteractability();

            if (ShipToInspect.IsFullyFuelled)
            {
                fuelButton.StopFueling();
            }
        }
    }

    private void Launch(bool isStartingMission)
    {
        if (ShipToInspect != null)
        {
            // Launch ship regardless of mission status 
            HangarManager.LaunchShip(UIManager.HangarNode);

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

    private bool IsStartMissionButtonInteractable()
    {
        if (ShipToInspect.CurrentMission != null)
        {
            return ShipToInspect.CurrentFuel >= ShipToInspect.CurrentMission.FuelCost
                && ShipToInspect.CurrentHullIntegrity > 0;
                //&& ShipToInspect.CanWarp;
        }
        return false;
    }

    private long GetFuelCostAfterLicences()
    {
        return Convert.ToInt64(fuelCostPerUnit * (1 - LicencesManager.FuelDiscountEffect));
    }

    private void SetBatteryChargeImage()
    {
        if (ShipToInspect.CanWarp)
        {
            batteryChargeAnimator.StartAnimation();
        }
        else
        {
            batteryChargeAnimator.StopAnimation();
        }
    }

    private void SetSliderValues()
    {
        if (ShipToInspect == null)
            return;

        fuelSlider.value = ShipToInspect.GetFuelPercentage();
        hullSlider.value = ShipToInspect.GetHullPercentage();
    }

    private void SetButtonInteractability()
    {
        fuelButton.Button.interactable = UIUtils.IsFuelButtonInteractable(ShipToInspect, fuelCostPerUnit);
        startMissionButton.interactable = IsStartMissionButtonInteractable();
        repairsButton.interactable = !ShipToInspect.IsFullyRepaired;
    }

    private void OnShipHealthChangedHandler(OnShipHealthChangedEvent healthChangedEvent)
    {
        SetSliderValues();
    }
}
