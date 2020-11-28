using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Direction
{
    Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight
}

public class PlayerMovement : MonoBehaviour
{
    public static Vector2 movementVector;

    [SerializeField] private SpriteAnimator spriteAnimator;

    private float currentSpeed; 
    [SerializeField] private float maximumSpeed; 
    [SerializeField] private float acceleration;

    private CharacterController characterController;
    private float gravity = -9.81f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); 
    }    

    // Get input in Update 
    private void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        SetDirection();
    }

    // Move player in FixedUpdate 
    private void FixedUpdate()
    {
        // Adjust for diagonal input 
        if (movementVector.magnitude > 1f)
        {
            movementVector /= movementVector.magnitude;
        }

        ApplyGravity();
        MovePlayer(); 
    }

    private void SetDirection()
    {
        if (movementVector == Vector2.up)
        {
            SpriteAnimator.direction = Direction.Up;
        }
        else if (movementVector == new Vector2(-1f, 1f))
        {
            SpriteAnimator.direction = Direction.UpLeft;
        }
        else if (movementVector == Vector2.left)
        {
            SpriteAnimator.direction = Direction.Left;
        }
        else if (movementVector == new Vector2(-1f, -1f))
        {
            SpriteAnimator.direction = Direction.DownLeft;
        }
        else if (movementVector == Vector2.down)
        {
            SpriteAnimator.direction = Direction.Down;
        }
        else if (movementVector == new Vector2(1f, -1f))
        {
            SpriteAnimator.direction = Direction.DownRight;
        }
        else if (movementVector == Vector2.right)
        {
            SpriteAnimator.direction = Direction.Right;
        }
        else if (movementVector == Vector2.one)
        {
            SpriteAnimator.direction = Direction.UpRight;
        }
    }

    private void ApplyGravity()
    {
        characterController.Move(new Vector3(0, gravity * Time.fixedDeltaTime, 0));
    }

    private void MovePlayer()
    {
        if (currentSpeed < maximumSpeed)
        {
            currentSpeed += acceleration; 
        }

        if (movementVector == Vector2.zero)
        {
            currentSpeed = 0f;
        }

        Vector3 movement = new Vector3(movementVector.x, 0f, movementVector.y);
        characterController.Move(movement * currentSpeed *Time.fixedDeltaTime);
    }

    private void LogMovementData()
    {
        Debug.Log("Movement vector: " + movementVector);
        Debug.Log("Current speed: " + currentSpeed);
        Debug.Log("Maximum speed: " + maximumSpeed);
    }
}
