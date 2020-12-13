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

    [SerializeField] private Animator animator;

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
            animator.SetBool("Up", true);
        }
        else if (movementVector == new Vector2(-1f, 1f))
        {
            animator.SetBool("Up", true);
        }
        else if (movementVector == Vector2.left)
        {
            animator.SetBool("Left", true);
        }
        else if (movementVector == new Vector2(-1f, -1f))
        {
            animator.SetBool("Left", true);
        }
        else if (movementVector == Vector2.down)
        {
            animator.SetBool("Down", true);
        }
        else if (movementVector == new Vector2(1f, -1f))
        {
            animator.SetBool("Down", true);
        }
        else if (movementVector == Vector2.right)
        {
            animator.SetBool("Right", true);
        }
        else if (movementVector == Vector2.one)
        {
            animator.SetBool("Right", true);
        }
        else
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
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
