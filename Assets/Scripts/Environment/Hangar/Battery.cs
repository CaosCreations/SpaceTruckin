using System;
using UnityEngine;

public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }
    
    [SerializeField] private MeshRenderer meshRenderer;
    private Color depletedEmission;
    private Color chargedEmission;

    [SerializeField] Rigidbody batteryRigidbody;

    // Shows that the player is holding any battery
    public static bool PlayerIsHoldingABattery;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (meshRenderer == null)
        {
            Debug.Log(
                $"Getting MeshRenderer on battery {gameObject.name} as the reference is missing on the prefab.");
            
            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (meshRenderer != null)
        {
            meshRenderer.material.EnableKeyword("_EMISSION");
            depletedEmission = meshRenderer.material.GetColor("_EmissionColor"); // Depleted by default
            chargedEmission = depletedEmission * HangarConstants.BatteryEmissionCoefficient;
        }
        else
        {
            Debug.LogError($"MeshRenderer on battery {gameObject.name} is null");
        }
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
    
    public void TakeBattery()
    {
        PlayerIsHoldingABattery = true;

        transform.parent = PlayerManager.PlayerObject.transform;

        ConfigureRigidbody(isConnectingToPlayer: true);
    }

    private void ConfigureRigidbody(bool isConnectingToPlayer)
    {
        // Disable gravity when a spring is connected
        batteryRigidbody.useGravity = !isConnectingToPlayer;

        if (isConnectingToPlayer)
        {
            batteryRigidbody.constraints = HangarConstants.BatteryRigidbodyConstraintsTaken;

            // Setting the container's position so that it floats above the ground
            transform.localPosition = new Vector3(
                transform.localPosition.x, HangarConstants.BatteryYPosition, transform.localPosition.z);
        }
        else
        {
            // As the battery is dropped, we remove the constraint so that the battery can move freely as a physics object
            batteryRigidbody.constraints = HangarConstants.BatteryRigidbodyConstraintsDropped;
        }
    }

    public void DropBattery()
    {
        PlayerIsHoldingABattery = false;

        // Re-configure the Rigidbody to be independent 
        ConfigureRigidbody(isConnectingToPlayer: false);
    }

    private void OnJointBreak(float breakForce)
    {
        DropBattery();
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerIsHoldingABattery == true)
        {
            // Don't let the player pick up a battery if they already have one
            return;
        }

        if (IsPlayerColliding && Input.GetKey(PlayerConstants.ActionKey))
        {
            TakeBattery();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.DropObjectKey))
        {
            DropBattery();
        }   
    }

    #region Persistence
    public const string FOLDER_NAME = "HangarSaveData";
    public const string FILE_NAME = "BatterySaveData";
    public static string FILE_PATH
    {
        get => DataUtils.GetSaveFilePath(FOLDER_NAME, FILE_NAME);
    }

    public void LoadData(BatterySaveData saveData)
    {
        transform.position = saveData.PositionInHangar;
        IsCharged = saveData.IsCharged;
        SetEmission();
    }
    #endregion
}

[Serializable]
public struct BatterySaveData
{
    public bool IsCharged;
    public Vector3 PositionInHangar;
}