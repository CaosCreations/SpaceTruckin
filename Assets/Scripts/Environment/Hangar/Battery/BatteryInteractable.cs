﻿using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class BatteryInteractable : InteractableObject
{
    [SerializeField] private Rigidbody batteryContainerRigidbody;

    [SerializeField] private Collider batteryModelCollider;

    // Shows that the player is holding any battery
    public static bool PlayerIsHoldingABattery;

    [SerializeField] private AnimatedObject_Player animatedObject_Player;


    public void TakeBattery()
    {
        PlayerIsHoldingABattery = true;

        gameObject.ParentToPlayer();

        // We place the battery above the player's head
        // We offset it's position towards the direction the player is facing

        transform.localPosition = new Vector3(0f, HangarConstants.BatteryYPosition, 0f);

        batteryModelCollider.enabled = false;

        ConfigureRigidbody(isConnectingToPlayer: true);

        HangarManager.CurrentBatteryBeingHeld = GetComponent<BatteryWrapper>();

        AnimationManager.Instance.PlayAnimation(animatedObject_Player, AnimationParameterType.BatteryGrab, isOn:true);
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
        PlayerIsHoldingABattery = false;

        ConfigureRigidbody(isConnectingToPlayer: false);

        gameObject.SetParent(HangarManager.BatteriesContainer);

        transform.position += PlayerManager.PlayerMovement.PlayerFacingDirection.normalized * 0.5f;

        batteryModelCollider.enabled = true;

        IsPlayerColliding = false;

        HangarManager.CurrentBatteryBeingHeld = null;

        AnimationManager.Instance.PlayAnimation(animatedObject_Player, AnimationParameterType.BatteryGrab, isOn: false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerIsHoldingABattery == true)
        {
            // Don't let the player pick up a battery if they already have one
            return;
        }

        if (!PlayerManager.IsPaused
          && IsPlayerColliding
          && Input.GetKey(PlayerConstants.ActionKey))
        {
            TakeBattery();
        }
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

        PlayerIsHoldingABattery = false;

        gameObject.SetParent(HangarManager.BatteriesContainer);

        HangarManager.CurrentBatteryBeingHeld = null;

        AnimationManager.Instance.PlayAnimation(animatedObject_Player, AnimationParameterType.BatteryGrab, isOn: false);
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
