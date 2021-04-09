using UnityEngine;

public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }

    [SerializeField] private GameObject batteryContainer; // Contains both colliders
    [SerializeField] private MeshRenderer meshRenderer;

    private Color depletedEmission;
    private Color chargedEmission;

    private void Start()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.EnableKeyword("_EMISSION");
            depletedEmission = meshRenderer.material.GetColor("_EmissionColor"); // Depleted by default
            chargedEmission = depletedEmission * HangarConstants.BatteryEmissionCoefficient;
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
}
