using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BatteryInteractable : InteractableObject
{
    [SerializeField] private Rigidbody batteryContainerRigidbody;

    [SerializeField] private Collider batteryModelCollider;

    // Shows that the player is holding any battery
    public static bool IsPlayerHoldingABattery;

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
        if (CanTakeBattery())
            TakeBattery();
    }

    /// <summary>
    /// Conditions for picking up a battery.
    /// </summary>
    private bool CanTakeBattery()
    {
        return !IsPlayerHoldingABattery
          && IsPlayerColliding
          && !PlayerManager.IsPaused
          && Input.GetKey(PlayerConstants.ActionKey)
          && PlayerManager.Raycast("Battery");
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

    private void Update()
    {
        if (!PlayerManager.IsPaused
            && Input.GetKey(PlayerConstants.DropObjectKey)
            && transform.parent.gameObject == PlayerManager.PlayerObject)
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
