﻿using Events;
using UnityEngine;

public class BatteryChargePoint : InteractableObject
{
    [SerializeField]
    private AnimationTimeHandler animationTimeHandler;

    private BatteryCharging currentBatteryCharging;

    [SerializeField]
    private StringBoolKeyValueEventArgs stringBoolKeyValueEventArgs;

    protected override void Start()
    {
        base.Start();
        animationTimeHandler.OnAnimationEnded += OnAnimationEndedHandler;
        animationTimeHandler.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(PlayerConstants.ActionKey) && IsPlayerInteractable)
        {
            BatteryCharging batteryCharging = other.GetComponentInChildren<BatteryCharging>();

            if (batteryCharging != null && !batteryCharging.IsCharged)
            {
                currentBatteryCharging = batteryCharging;
                animationTimeHandler.SetActive(true);
                animationTimeHandler.HandleAnimation();
            }
        }
    }

    private void OnAnimationEndedHandler()
    {
        if (currentBatteryCharging == null)
            return;

        currentBatteryCharging.Charge();
        animationTimeHandler.SetActive(false);
        currentBatteryCharging = null;
        SingletonManager.EventService.Dispatch(new OnBatteryChargedEvent(stringBoolKeyValueEventArgs));
    }

    protected override bool IsIconVisible =>
        IsPlayerInteractable
        && HangarManager.CurrentBatteryBeingHeld != null
        && !HangarManager.CurrentBatteryBeingHeld.BatteryCharging.IsCharged;
}
