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
    [SerializeField] private GameObject batteryContainer; // Contains both colliders
    [SerializeField] private MeshRenderer meshRenderer;

    public bool IsCharged { get => saveData.isCharged; set => saveData.isCharged = value; }
    public Vector3 PositionInHangar 
    {
        get => saveData.positionInHangar; set => saveData.positionInHangar = value; 
    }

    private BatterySaveData saveData;

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
        saveData.positionInHangar = transform.position;
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

    public string GetJson()
    {
        return JsonUtility.ToJson(saveData);
    }
    #endregion
}
