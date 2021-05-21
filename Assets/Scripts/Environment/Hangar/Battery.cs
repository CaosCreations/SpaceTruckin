using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }
    public GameObject Container; // Contains both colliders
    
    [SerializeField] private MeshRenderer meshRenderer;
    private Color depletedEmission;
    private Color chargedEmission;

    [SerializeField] private BatterySpawnPositionManager batterySpawnPositionManager;

    [SerializeField] private BoxCollider boxCollider;

    private void Awake()
    {
        Init();
        if (boxCollider == null)
            Debug.LogError("boxCollider is null. Please assign assign the current Boxcollider to this variable." +
                           "We need it to check collisions for various things: respawning the battery, " +
                            "checking when the battery exits the hangar, dropping it on the ground");
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

    public bool PlayerIsHolding()
    {
        return PlayerManager.PlayerObject.GetComponentInChildren<Battery>() != null;
    }

    public void TakeBattery()
    {
        Container.ParentToPlayer();
    }

    public void DropBattery()
    {
        Container.SetParent(HangarManager.BatteriesContainer);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!PlayerManager.IsPaused
            && IsPlayerColliding
            && Input.GetKey(PlayerConstants.ActionKey))
        {
            if (PlayerIsHolding())
            {
                // Don't let the player pick up a battery if they already have one
                return;
            }
            TakeBattery();
        }
    }

    // To prevent the player exits the hangar with a battery, we respawn it back into the hangar
    public override void OnTriggerExit(Collider other)
    {
        if(PlayerIsColliding(other))
        {
            IsPlayerColliding = false;
            return;
        }

        if (other.CompareTag(HangarConstants.BatteryExitColliderTag))
        {
            DropBattery();
            batterySpawnPositionManager.RespawnBattery(Container.transform, boxCollider);
            IsPlayerColliding = false;
        }
    }

    private void Update()
    {
        if (!PlayerManager.IsPaused 
            && Input.GetKeyDown(PlayerConstants.DropObjectKey)
            && PlayerIsHolding())
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