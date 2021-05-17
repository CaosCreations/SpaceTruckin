using System;
using UnityEngine;

public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }
    
    [SerializeField] private MeshRenderer meshRenderer;
    private Color depletedEmission;
    private Color chargedEmission;

    private FixedJoint fixedJoint;
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

        // We position the battery away from the player. If we don't do this the player won't have space to move towards the battery.
        // So the battery won't move, blocking the player.
        // We must do this before the creating the fixed spring, because once a fixed spring is set you 
        // can't change the distance between the 2 attached bodies.

        Vector3 playertoBatteryDirection = (new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(PlayerManager.PlayerObject.transform.position.x, 0f, PlayerManager.PlayerObject.transform.position.z)).normalized;
        transform.position += playertoBatteryDirection * 0.25f;

        ConfigureFixedJoint(); 

        // Update the Rigidbody settings to align with the spring physics 
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

    private void ConfigureFixedJoint()
    {
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = PlayerManager.PlayerObject.GetComponent<Rigidbody>();

        // We want the fixed joint to break when the battery collides with other objects (walls, etc.)
        fixedJoint.breakForce = HangarConstants.BatteryBreakForce;
        fixedJoint.enableCollision = HangarConstants.BatteryEnableCollision;
    }

    public void DropBattery()
    {
        PlayerIsHoldingABattery = false;

        if (fixedJoint != null)
        {
            Destroy(fixedJoint);
        }

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