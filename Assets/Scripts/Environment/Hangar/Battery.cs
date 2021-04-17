using UnityEngine;

public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }

    [SerializeField] private GameObject batteryContainer; // Contains both colliders
    [SerializeField] private GameObject batteryModel;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody myRigidbody;

    private Color depletedEmission;
    private Color chargedEmission;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (meshRenderer == null)
        {
            Debug.Log(
                $"Getting MeshRenderer on battery {gameObject.name} as the reference is missing on the prefab.");
            
            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (meshRenderer != null)
        {
            InitEmission();
        }
        else
        {
            Debug.LogError($"MeshRenderer on battery {gameObject.name} is null");
        }
    }

    private void InitEmission()
    {
        meshRenderer.material.EnableKeyword("_EMISSION");
        depletedEmission = meshRenderer.material.GetColor("_EmissionColor"); // Depleted by default
        chargedEmission = depletedEmission * HangarConstants.BatteryEmissionCoefficient;
    }

    public void Charge()
    {
        IsCharged = true;
        SetEmission();
    }

    public void Discharge()
    {
        IsCharged = false;
        SetEmission();
    }

    private void SetEmission()
    {
        Color emission = IsCharged ? chargedEmission : depletedEmission;
        meshRenderer.material.SetColor("_EmissionColor", emission);
    }

    public bool PlayerIsHoldingABattery()
    {
        return PlayerManager.PlayerObject.GetComponentInChildren<Battery>() != null;
    }

    public bool PlayerIsHoldingThisBattery()
    {
        return PlayerManager.PlayerObject.GetComponentInChildren<Battery>() == this;
    }

    public void TakeBattery()
    {
        batteryContainer.ParentToPlayer();
        PlayerManager.PlayerMovement.ConnectBodyToSpring(myRigidbody);
    }

    public void DropBattery()
    {
        batteryContainer.SetParent(HangarManager.BatteriesContainer);
        PlayerManager.PlayerMovement.DisconnectBodyFromSpring();
    }

    private void RotateBattery()
    {
        batteryModel.transform.Rotate(new Vector3(0f, 0f, HangarConstants.BatteryRotationSpeed));
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerColliding && Input.GetKey(PlayerConstants.ActionKey))
        {
            if (PlayerIsHoldingABattery())
            {
                // Don't let the player pick up a battery if they already have one
                return;
            }
            TakeBattery();
        }
    }

    private void Update()
    {
        if (PlayerIsHoldingThisBattery())
        {
            RotateBattery();
        }
        
        if (PlayerIsHoldingABattery() && Input.GetKeyDown(PlayerConstants.DropObjectKey))
        {
            DropBattery();
        }
    }
}
