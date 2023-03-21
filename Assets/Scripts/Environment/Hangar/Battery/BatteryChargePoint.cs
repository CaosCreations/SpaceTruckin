using UnityEngine;

public class BatteryChargePoint : InteractableObject
{
    [SerializeField]
    private float chargeTimeInSeconds = 2f;

    [SerializeField]
    private AnimationTimeHandler animationTimeHandler;

    private BatteryCharging currentBatteryCharging;

    protected override void Start()
    {
        base.Start();
        animationTimeHandler.OnAnimationEnded += OnAnimationEndedHandler;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(PlayerConstants.ActionKey) && IsPlayerInteractable)
        {
            BatteryCharging batteryCharging = other.GetComponentInChildren<BatteryCharging>();

            if (batteryCharging != null && !batteryCharging.IsCharged)
            {
                currentBatteryCharging = batteryCharging;
                animationTimeHandler.HandleAnimation();
            }
        }
    }

    private void OnAnimationEndedHandler()
    {
        if (currentBatteryCharging == null)
            return;

        currentBatteryCharging.Charge();
    }

    protected override bool IsIconVisible =>
        IsPlayerInteractable
        && HangarManager.CurrentBatteryBeingHeld != null
        && !HangarManager.CurrentBatteryBeingHeld.BatteryCharging.IsCharged;

    private void OnValidate()
    {
        chargeTimeInSeconds = Mathf.Max(0f, chargeTimeInSeconds);
    }
}
