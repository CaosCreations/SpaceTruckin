using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HangarNodeUI : MonoBehaviour
{
    [Header("Set In Editor")]
    public GameObject mainPanel;
    public GameObject repairPanel;
    public GameObject upgradePanel;

    public Slider fuelSlider;
    public FuelButton fuelButton;

    public Slider hullSlider;
    public Button hullButton;

    public Button upgradeButton;
    public Button launchButton;

    [Header("Set at Runtime")]
    public GameObject shipPreview;
    public HangarNode hangarNode;
    public Ship shipToInspect;

    private bool isInMenu;
    private long fuelCostPerUnit = 1;

    private void OnEnable()
    {
        hangarNode = UIManager.Instance.hangarNode;
        shipToInspect = ShipsManager.GetShipForNode(hangarNode);

        if(shipToInspect == null)
        {
            gameObject.SetActive(false);
        }

        PopulateUI();
    }

    private void OnDisable()
    {
        Destroy(shipPreview);
    }

    void Update()
    {
        if (isInMenu)
        {
            if (Input.GetKeyDown(PlayerConstants.exit))
            {
                QuitMenu();
            }
        }
        else
        {
            if (shipPreview != null)
            {
                shipPreview.transform.Rotate(new Vector3(0, 0.01f, 0));
            }
        }

        CheckFueling();
    }

    void PopulateUI()
    {
        QuitMenu();
        shipPreview = Instantiate(shipToInspect.shipPrefab);

        fuelSlider.value = shipToInspect.GetFuelPercent();

        hullSlider.value = shipToInspect.GetHullPercent();
        hullButton.onClick.RemoveAllListeners();
        hullButton.onClick.AddListener(Repair);

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(Upgrade);

        launchButton.onClick.RemoveAllListeners();
        launchButton.onClick.AddListener(Launch);
    }

    private void CheckFueling()
    {
        if (fuelButton.isFueling)
        {
            if (shipToInspect.currentFuel < shipToInspect.maxFuel
             && PlayerManager.Instance.playerData.SpendMoney(fuelCostPerUnit))
            {
                shipToInspect.currentFuel++;
                fuelSlider.value = shipToInspect.GetFuelPercent();
            }
        }
    }

    private void Repair()
    {
        mainPanel.SetActive(false);
        repairPanel.SetActive(true);
        upgradePanel.SetActive(true);
        isInMenu = true;
        UIManager.Instance.currentMenuOverridesEscape = true;
    }

    private void Upgrade()
    {
        mainPanel.SetActive(false);
        repairPanel.SetActive(false);
        upgradePanel.SetActive(true);
        isInMenu = true;
        UIManager.Instance.currentMenuOverridesEscape = true;
    }

    private void Launch()
    {
        if (shipToInspect.currentFuel > 0)
        {
            ShipsManager.LaunchShip(hangarNode);
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Ship has no fuel!");
        }
        
    }

    private void QuitMenu()
    {
        mainPanel.SetActive(true);
        repairPanel.SetActive(false);
        upgradePanel.SetActive(false);
        isInMenu = false;
        UIManager.Instance.currentMenuOverridesEscape = false;
    }

    
}
