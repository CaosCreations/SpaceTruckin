using UnityEngine;

public class Battery : InteractableObject
{
    public bool IsCharged { get; set; }

    [SerializeField] private GameObject batteryContainer; // Contains both colliders
    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        SetColour();
    }

    public void Charge()
    {
        IsCharged = true;
        SetColour();
    }

    public void Discharge()
    {
        IsCharged = false;
        SetColour();
    }

    private void SetColour()
    {
        meshRenderer.material.color = IsCharged ?
            HangarConstants.ChargedBatteryColour :
            HangarConstants.DepletedBatteryColour;
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
