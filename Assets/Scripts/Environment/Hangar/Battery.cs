using System;
using UnityEngine;

public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }
    
    [SerializeField] private MeshRenderer meshRenderer;
    private Color depletedEmission;
    private Color chargedEmission;

    [SerializeField] Rigidbody batteryContainerRigidbody;
    [SerializeField] Collider batteryModelCollider;

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

        gameObject.ParentToPlayer();

        // We place the battery above the player's head
        // We offset it's position towards the direction the player is facing

        transform.localPosition = new Vector3(0f, HangarConstants.BatteryYPosition, 0f);

        batteryModelCollider.enabled = false;

        ConfigureRigidbody(isConnectingToPlayer: true);
    }

    private void ConfigureRigidbody(bool isConnectingToPlayer)
    {
        batteryContainerRigidbody.useGravity = !isConnectingToPlayer;

        if (isConnectingToPlayer)
        {
            batteryContainerRigidbody.constraints = HangarConstants.BatteryRigidbodyConstraintsTaken;
        }
        else
        {
            // As the battery is dropped, we remove the constraint so that the battery can move freely as a physics object
            batteryContainerRigidbody.constraints = HangarConstants.BatteryRigidbodyConstraintsDropped;
        }
    }

    public void DropBattery()
    {
        PlayerIsHoldingABattery = false;

        ConfigureRigidbody(isConnectingToPlayer: false);

        gameObject.SetParent(HangarManager.BatteriesContainer);

        transform.position += PlayerManager.Instance.PlayerFacingDirection.normalized *0.5f;

        batteryModelCollider.enabled = true;
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
        if (Input.GetKeyDown(PlayerConstants.DropObjectKey) && transform.parent.gameObject == PlayerManager.PlayerObject)
        {
            DropBattery();
        }

        Debug.Log("Player facing direction = " +PlayerManager.Instance.PlayerFacingDirection);
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