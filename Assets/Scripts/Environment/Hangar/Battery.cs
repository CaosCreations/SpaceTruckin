using System;
using UnityEngine;

[Serializable]
public struct BatterySaveData
{
    public bool isCharged;
    public Vector3 positionInHangar;
}

public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }
    [SerializeField] private GameObject batteryContainer; // Contains both colliders
    
    [SerializeField] private MeshRenderer meshRenderer;
    private Color depletedEmission;
    private Color chargedEmission;

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
        batteryContainer.ParentToPlayer();
    }

    public void DropBattery()
    {
        batteryContainer.SetParent(HangarManager.BatteriesContainer);
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerColliding && Input.GetKey(PlayerConstants.ActionKey))
        {
            if (PlayerIsHolding())
            {
                // Don't let the player pick up a battery if they already have one
                return;
            }
            TakeBattery();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.DropObjectKey)
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
        batteryContainer.transform.position = saveData.positionInHangar;
        IsCharged = saveData.isCharged;
        SetEmission();
    }
    #endregion
}
