using System;
using UnityEngine;

public enum Direction
{
    Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight
}

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 MovementVector;
    public Vector3 PlayerFacingDirection;

    public Rigidbody PlayerRigidbody;
    private CharacterController characterController;
    public PlayerMovementAnimation MovementAnimation;

    public float CurrentSpeed { get; private set; }
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float acceleration;

    private void Start()
    {
        if (!TryGetComponent(out characterController))
        {
            throw new NullReferenceException("Character controller component not found in Player Movement script.");
        }
    }

    // Get input in Update 
    private void Update()
    {
        if (PlayerManager.IsPaused || DialogueUtils.IsConversationActive)
        {
            return;
        }

        if (IsPlayerBelowKillFloor() || Input.GetKeyDown(PlayerConstants.RespawnKey))
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

        MovementAnimation.SetParams();
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

    private void ApplyGravity()
    {
        characterController.Move(new Vector3(0, PlayerConstants.Gravity * Time.fixedDeltaTime, 0));
    }

    private void DetermineSpeed()
    {
        if (Input.GetKey(PlayerConstants.SprintKey) && !MovementAnimation.BabyMode && !BatteryInteractable.IsPlayerHoldingABattery)
        {
            CurrentSpeed = PlayerConstants.RunSpeed;
        }
        else
        {
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
