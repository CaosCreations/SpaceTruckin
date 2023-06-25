using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BatteryInteractable : InteractableObject
{
    [SerializeField] private Rigidbody batteryContainerRigidbody;

    [SerializeField] private Collider batteryModelCollider;

    // Shows that the player is holding any battery
    public static bool IsPlayerHoldingABattery;

    private bool cannotDrop;

    public void TakeBattery()
    {
        IsPlayerHoldingABattery = true;

        gameObject.ParentToPlayer();

        // We place the battery above the player's head
        // We offset it's position towards the direction the player is facing
        transform.localPosition = new Vector3(0f, HangarConstants.BatteryYPosition, 0f);

        batteryModelCollider.enabled = false;

        ConfigureRigidbody(isConnectingToPlayer: true);

        HangarManager.CurrentBatteryBeingHeld = GetComponent<BatteryWrapper>();

        PlayerAnimationManager.Instance.PlayAnimation(PlayerAnimationParameterType.BatteryGrab, isOn: true);
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
        IsPlayerHoldingABattery = false;

        ConfigureRigidbody(isConnectingToPlayer: false);

        gameObject.SetParent(HangarManager.BatteriesContainer);

        transform.position += PlayerManager.PlayerMovement.PlayerFacingDirection.normalized * 0.5f;

        batteryModelCollider.enabled = true;

        IsPlayerColliding = false;

        HangarManager.CurrentBatteryBeingHeld = null;

        PlayerAnimationManager.Instance.PlayAnimation(PlayerAnimationParameterType.BatteryGrab, isOn: false);
    }

    private void OnTriggerStay(Collider other)
    {
        // Override drop behaviour if other interactables should take priority
        if (other.CompareTag(HangarConstants.BatteryChargerTag))
        {
            var charger = other.GetComponent<BatteryChargePoint>();
            if (charger != null && charger.IsPlayerInteractable)
            {
                cannotDrop = true;
                return;
            }
        }

        if (other.CompareTag(HangarConstants.BatterySlotTag))
        {
            var slot = other.GetComponent<BatterySlot>();
            if (slot != null && slot.IsPlayerInteractable)
            {
                cannotDrop = true;
                return;
            }
        }
        cannotDrop = false;
    }

    /// <summary>
    /// Conditions for picking up a battery.
    /// </summary>
    private bool CanTakeBattery()
    {
        return !IsPlayerHoldingABattery
          && !PlayerManager.IsPaused
          && Input.GetKeyDown(PlayerConstants.ActionKey)
          && IsPlayerInteractable;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag(HangarConstants.BatteryExitColliderTag))
        {
            DropBattery();
            HangarManager.BatterySpawnPositionManager.RespawnBattery(transform, Collider);
        }
    }

    public void PlaceBatteryInSlot(Transform slot)
    {
        transform.position = slot.transform.position;

        IsPlayerHoldingABattery = false;

        gameObject.SetParent(HangarManager.BatteriesContainer);

        HangarManager.CurrentBatteryBeingHeld = null;

        PlayerAnimationManager.Instance.PlayAnimation(PlayerAnimationParameterType.BatteryGrab, isOn: false);
    }

    protected override bool IsIconVisible => IsPlayerInteractable && !IsPlayerHoldingABattery;

    private bool CanDropBattery()
    {
        return !cannotDrop
            && transform.parent.gameObject == PlayerManager.PlayerObject
            && !PlayerManager.IsPaused
            && Input.GetKeyDown(PlayerConstants.ActionKey);
    }

    protected override void Update()
    {
        base.Update();

        if (CanTakeBattery())
        {
            TakeBattery();
        }
        else if (CanDropBattery())
        {
            DropBattery();
        }
    }

    // At the moment, as the scene is loaded, the position of the battery is defined by the SpawnPosition Manager not the HangarManager
    // So this function is never called
    public void LoadData(BatterySaveData saveData)
    {
        transform.position = saveData.PositionInHangar;
    }
}
