using System;
using UnityEngine;

public enum Direction
{
    Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight
}

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 MovementVector;

    public Rigidbody PlayerRigidbody;

    [SerializeField] private Animator animator;
    private CharacterController characterController;

    public float CurrentSpeed { get; private set; }
    private bool CanRun => !BatteryInteractable.IsPlayerHoldingABattery;

    [SerializeField] private float maximumSpeed;
    [SerializeField] private float acceleration;

    public Vector3 PlayerFacingDirection;

    private void Start()
    {
        if (!TryGetComponent(out characterController))
        {
            throw new MissingReferenceException("Character controller component not found in Player Movement script.");
        }
    }

    // Get input in Update 
    private void Update()
    {
        if (PlayerManager.IsPaused)
        {
            return;
        }

        if (IsPlayerBelowKillFloor()
            || Input.GetKeyDown(PlayerConstants.RespawnKey))
        {
            RespawnPlayer();
            return;
        }

        MovementVector.x = Input.GetAxisRaw("Horizontal");
        MovementVector.y = Input.GetAxisRaw("Vertical");

        // If the player is not moving then then Movement Vector in PlayerMovement is 0
        // As we want a direction we only take the last non 0 Movement Vector

        if (MovementVector != Vector3.zero)
        {
            PlayerFacingDirection = new Vector3(MovementVector.x, 0f, MovementVector.y);
        }

        SetDirection();
    }

    // Move player in FixedUpdate 
    private void FixedUpdate()
    {
        if (PlayerManager.IsPaused)
        {
            return;
        }

        // Adjust for diagonal input 
        if (MovementVector.magnitude > 1f)
        {
            MovementVector /= MovementVector.magnitude;
        }

        ApplyGravity();
        DetermineSpeed();
        MovePlayer();

        Debug.DrawRay(transform.position, PlayerFacingDirection, Color.yellow);
    }

    private void SetDirection()
    {
        ResetDirection();

        if (AnimationConstants.MovementAnimationMap.ContainsKey(MovementVector))
        {
            // Get the matching parameters for the player's current direction  
            string[] activeParams = AnimationConstants.MovementAnimationMap[MovementVector];

            // Update the state machine 
            Array.ForEach(activeParams, x => animator.SetBool(x, true));
        }
    }

    public void ResetDirection()
    {
        animator.SetBool(AnimationConstants.AnimationUpParameter, false);
        animator.SetBool(AnimationConstants.AnimationLeftParameter, false);
        animator.SetBool(AnimationConstants.AnimationDownParameter, false);
        animator.SetBool(AnimationConstants.AnimationRightParameter, false);
    }

    private void ApplyGravity()
    {
        characterController.Move(new Vector3(0, PlayerConstants.Gravity * Time.fixedDeltaTime, 0));
    }

    private void DetermineSpeed()
    {
        if (CanRun && Input.GetKey(PlayerConstants.SprintKey))
        {
            animator.SetBool(AnimationConstants.AnimationRunParameter, true);
            CurrentSpeed = PlayerConstants.RunSpeed;
        }
        else
        {
            animator.SetBool(AnimationConstants.AnimationRunParameter, false);
            CurrentSpeed = PlayerConstants.WalkSpeed;
        }
    }

    private void MovePlayer()
    {
        if (CurrentSpeed < maximumSpeed)
        {
            CurrentSpeed += acceleration;
        }

        if (MovementVector == Vector3.zero)
        {
            CurrentSpeed = 0f;
        }

        Vector3 movement = new(MovementVector.x, 0f, MovementVector.y);
        characterController.Move(CurrentSpeed * Time.fixedDeltaTime * movement);
    }

    private bool IsPlayerBelowKillFloor()
    {
        return characterController.transform.position.y < PlayerConstants.KillFloorHeight;
    }

    private void ResetPlayerPosition()
    {
        characterController.enabled = false;
        transform.position = PlayerConstants.PlayerResetPosition;
        characterController.enabled = true;
    }

    private void RespawnPlayer()
    {
        ResetPlayerPosition();
        CounteractRespawnSideEffects();
    }

    /// <summary>
    /// Stop respawning affecting other game state in undesired ways.
    /// </summary>
    private void CounteractRespawnSideEffects()
    {
        // Stop UI canvas remaining interactable 
        UIManager.SetCannotInteract();

        // Stop door remaining open 
        OfficeDoor collidingDoor = EnvironmentUtils.GetDoorCollidingWithPlayer();

        if (collidingDoor != null)
        {
            collidingDoor.CloseDoor();
        }
    }

    public bool Raycast(string layerName, out RaycastHit hit)
    {
        LayerMask layerMask = LayerMask.GetMask(layerName);
        return Physics.Raycast(transform.position, PlayerFacingDirection, out hit, PlayerConstants.RaycastDistance, layerMask);
    }

    public bool IsFirstRaycastHit(GameObject obj)
    {
        var layerName = LayerMask.LayerToName(obj.layer);
        if (!Raycast(layerName, out RaycastHit hit))
            return false;

        return hit.collider != null && hit.collider.gameObject == obj;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
