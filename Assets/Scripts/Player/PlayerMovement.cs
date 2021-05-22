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

    private float currentSpeed;
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float acceleration;

    // Player movement relates to camera
    public Transform CameraTransform;

    public delegate void UpdateMovementVectorDelegate(Vector3 movementVector);

    public UpdateMovementVectorDelegate UpdateMovementVectorEvent;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Initialize the player's Camera
        if (Camera.main != null)
        {
            CameraTransform = Camera.main.transform;
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

        Debug.Log("MovementVector = " + MovementVector);
        UpdateMovementVectorEvent(MovementVector);

        SetDirection();

        RotateWithView(MovementVector, CameraTransform);
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
    }

    private void SetDirection()
    {
        ResetDirection();

        if (PlayerConstants.MovementAnimationMap.ContainsKey(MovementVector))
        {
            // Get the matching parameters for the player's current direction  
            string[] activeParams = PlayerConstants.MovementAnimationMap[MovementVector];

            // Update the state machine 
            Array.ForEach(activeParams, x => animator.SetBool(x, true));
        }
    }

    public void ResetDirection()
    {
        animator.SetBool(PlayerConstants.AnimationUpParameter, false);
        animator.SetBool(PlayerConstants.AnimationLeftParameter, false);
        animator.SetBool(PlayerConstants.AnimationDownParameter, false);
        animator.SetBool(PlayerConstants.AnimationRightParameter, false);
    }

    private void ApplyGravity()
    {
        characterController.Move(new Vector3(0, PlayerConstants.Gravity * Time.fixedDeltaTime, 0));
    }

    private void DetermineSpeed()
    {
        if (Input.GetKey(PlayerConstants.SprintKey))
        {
            animator.SetBool(PlayerConstants.AnimationRunParameter, true);
            currentSpeed = PlayerConstants.RunSpeed;
        }
        else
        {
            animator.SetBool(PlayerConstants.AnimationRunParameter, false);
            currentSpeed = PlayerConstants.WalkSpeed;
        }
    }

    private void MovePlayer()
    {
        if (currentSpeed < maximumSpeed)
        {
            currentSpeed += acceleration;
        }

        if (MovementVector == Vector3.zero)
        {
            currentSpeed = 0f;
        }

        Vector3 movement = new Vector3(MovementVector.x, 0f, MovementVector.y);
        characterController.Move(movement * currentSpeed * Time.fixedDeltaTime);
    }

    private bool IsPlayerBelowKillFloor()
    {
        return characterController.transform.position.y < PlayerConstants.KillFloorHeight;
    }

    private void ResetPlayerToOrigin()
    {
        characterController.enabled = false;
        transform.position = PlayerConstants.PlayerResetPosition;
        characterController.enabled = true;
    }

    private void RespawnPlayer()
    {
        ResetPlayerToOrigin();
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

    public static void RotateWithView(Vector3 vector, Transform cameraTransform)
    {
        Vector3 dir = cameraTransform.TransformDirection(vector);
        dir.Set(dir.x, 0, dir.z);
        vector = dir.normalized * vector.magnitude;
    }
}
