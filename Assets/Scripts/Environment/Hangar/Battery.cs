﻿using System;
using UnityEngine;

public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }
    public GameObject Container; // Contains both colliders
    
    [SerializeField] private MeshRenderer meshRenderer;
    private Color depletedEmission;
    private Color chargedEmission;

    private SpringJoint springJoint;
    [SerializeField] Rigidbody containerRigidBody;

    // Shows that the player is holding any battery
    public static bool PlayerIsHoldingBattery;

    // Shows that this instance of the battery is being held by the player
    private bool springJointIsSet;

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
        springJointIsSet = true;

        PlayerIsHoldingBattery = true;

        containerRigidBody.useGravity = false;

        // We add constraints so that the battery doesn't swing around uncontrollably
        containerRigidBody.constraints = HangarConstants.BatteryRigidbodyConstraintsTaken;

        // Setting the container's spring joint values
        Container.transform.localPosition = new Vector3(Container.transform.localPosition.x, HangarConstants.BatteryYPosition, Container.transform.localPosition.z);

        ConfigureSpringJoint(); 
    }

    private void ConfigureSpringJoint()
    {
        springJoint = Container.AddComponent<SpringJoint>();
        springJoint.connectedBody = PlayerManager.PlayerObject.GetComponent<Rigidbody>();
        springJoint.spring = HangarConstants.Spring;
        springJoint.damper = HangarConstants.Damper;
        springJoint.minDistance = HangarConstants.MinDistance;
        springJoint.maxDistance = HangarConstants.MaxDistance;
        springJoint.tolerance = HangarConstants.Tolerance;
        springJoint.enableCollision = HangarConstants.EnableCollision;
        springJoint.breakForce = HangarConstants.BreakForce;
    }

    public void DropBattery()
    {
        // As the battery is dropped, we remove the constraint so that the battery can move freely as a physics object
        containerRigidBody.constraints = HangarConstants.BatteryRigidbodyConstraintsDropped;
        containerRigidBody.useGravity = true;
        Destroy(springJoint);

        PlayerIsHoldingBattery = false;
        springJointIsSet = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerIsHoldingBattery == true)
        {
            // Don't let the player pick up a battery if they already have one
            return;
        }

        else if (IsPlayerColliding && Input.GetKey(PlayerConstants.ActionKey))
        {
            TakeBattery();
        }
    }

    private void Update()
    {
        if((springJointIsSet == true 
            && (Container.GetComponent<SpringJoint>() == false) 
            || Input.GetKeyDown(PlayerConstants.DropObjectKey)))
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
        Container.transform.position = saveData.PositionInHangar;
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