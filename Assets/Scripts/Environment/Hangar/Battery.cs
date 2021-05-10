using System;
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

    public static bool PlayerIsHoldingBattery;

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

    /*public bool PlayerIsHolding()
    {
        //Debug.Log("PlayerIsHolding = " + PlayerManager.PlayerObject.GetComponentInChildren<Battery>() != null);

        return springJoint != null && springJoint.connectedBody == PlayerManager.PlayerMovement.PlayerRigidbody;
    }
    */
    

    public void TakeBattery()
    {
        springJointIsSet = true;

        PlayerIsHoldingBattery = true;

        Container.transform.localPosition = new Vector3(Container.transform.localPosition.x, HangarConstants.BatteryYPosition, Container.transform.localPosition.z);

        containerRigidBody.useGravity = false;

        // We add constraints so that the battery doesn't swing around uncontrollably
        containerRigidBody.constraints = HangarConstants.BatteryRigidbodyConstraints;

        // Setting the container's spring joint values

        ConfigureSpringJoint();

        springJoint.damper = HangarConstants.Damper;
        springJoint.minDistance = HangarConstants.MinDistance;
        springJoint.maxDistance = HangarConstants.MaxDistance;
        springJoint.tolerance = HangarConstants.Tolerance;
        springJoint.enableCollision = HangarConstants.EnableCollision;
        springJoint.breakForce = HangarConstants.BreakForce;
    }

    private void ConfigureSpringJoint()
    {
        springJoint = Container.AddComponent<SpringJoint>();
        springJoint.connectedBody = PlayerManager.PlayerObject.GetComponent<Rigidbody>();
        springJoint.spring = HangarConstants.Spring;
    }

    public void DropBattery()
    {
        springJointIsSet = false;
        // As the battery is dropped, we remove the constraint so that the battery can move freely as a physics object
        containerRigidBody.constraints = RigidbodyConstraints.None;
        containerRigidBody.useGravity = true;
        Destroy(springJoint);
        PlayerIsHoldingBattery = false;
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
        Debug.Log("PlayerIsHoldingBattery =" + PlayerIsHoldingBattery);

        if(springJointIsSet == true && Container.GetComponent<SpringJoint>() == false)
        {
            DropBattery();
        }

        if (Input.GetKeyDown(PlayerConstants.DropObjectKey)
            && PlayerIsHoldingBattery == true)
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