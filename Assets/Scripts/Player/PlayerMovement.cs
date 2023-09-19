using Cinemachine;
using System;
using UnityEngine;

public enum Direction
{
    Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight
}

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 MovementVector;
    public static Vector3 PlayerFacingDirection;

    public Rigidbody PlayerRigidbody;
    private CharacterController characterController;
    public PlayerMovementAnimation MovementAnimation;

    public float CurrentSpeed { get; private set; }
    public bool IsRunning => CurrentSpeed == PlayerConstants.RunSpeed;
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
            SetPosition(PlayerConstants.PlayerRespawnPosition, AnimationConstants.OfficeCameraStateName);
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
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && !MovementAnimation.IsHoldingBaby && !BatteryInteractable.IsPlayerHoldingABattery)
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

    public void SetPosition(Vector3 position, string cameraStateName = null)
    {
        if (cameraStateName != null)
        {
            StationCameraManager.Instance.SetBlend(CinemachineBlendDefinition.Style.Cut, 0f);
        }

        characterController.enabled = false;
        transform.position = position;
        characterController.enabled = true;

        // Stop UI canvas remaining interactable 
        UIManager.SetCannotInteract();

        // Stop door remaining open 
        OfficeDoor collidingDoor = EnvironmentUtils.GetDoorCollidingWithPlayer();

        if (collidingDoor != null)
        {
            collidingDoor.CloseDoor();
        }

        if (cameraStateName != null)
        {
            StationCameraManager.Instance.PlayCamAnimState(cameraStateName);
            StationCameraManager.Instance.SetBlend(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        }
    }

    public void FlipFacingDirection()
    {
        PlayerFacingDirection = -PlayerFacingDirection;
    }

    public void StopPlayer()
    {
        MovementVector = Vector3.zero;
    }
}
