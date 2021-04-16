using UnityEngine;

public enum Direction
{
    Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight
}

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 MovementVector;
    private readonly float killFloorHeight = -25;
    private readonly Vector3 playerResetPosition = new Vector3(210f, 380f, -247f);

    [SerializeField] private Animator animator;

    private float currentSpeed;
    private readonly float walkSpeed = 3f;
    private readonly float runSpeed = 7f;
    [SerializeField] private float maximumSpeed; 
    [SerializeField] private float acceleration;

    private CharacterController characterController;
    private readonly float gravity = -9.81f;

    // Player movement relates to camera
    public Transform CameraTransform;

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
        if (IsPlayerBelowKillFloor())
        {
            ResetPlayerToOrigin();
            return;

        }

        MovementVector.x = Input.GetAxisRaw("Horizontal");
        MovementVector.y = Input.GetAxisRaw("Vertical");

        SetDirection();
        RotateWithView(MovementVector,CameraTransform);
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
        MovePlayer(); 
    }

    private void SetDirection()
    {
        if (MovementVector == Vector3.up)
        {
            animator.SetBool("Up", true);
        }
        else if (MovementVector == new Vector3(-1f, 1f))
        {
            animator.SetBool("Up", true);
        }
        else if (MovementVector == Vector3.left)
        {
            animator.SetBool("Left", true);
        }
        else if (MovementVector == new Vector3(-1f, -1f))
        {
            animator.SetBool("Left", true);
        }
        else if (MovementVector == Vector3.down)
        {
            animator.SetBool("Down", true);
        }
        else if (MovementVector == new Vector3(1f, -1f))
        {
            animator.SetBool("Down", true);
        }
        else if (MovementVector == Vector3.right)
        {
            animator.SetBool("Right", true);
        }
        else if (MovementVector == Vector3.one)
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

        if (Input.GetKey(KeyCode.W)) animator.SetBool("KeyUp", true);
        else animator.SetBool("KeyUp", false);

        if (Input.GetKey(KeyCode.S)) animator.SetBool("KeyDown", true);
        else animator.SetBool("KeyDown", false);

        if (Input.GetKey(KeyCode.D)) animator.SetBool("KeyRight", true);
        else animator.SetBool("KeyRight", false);

        if (Input.GetKey(KeyCode.A)) animator.SetBool("KeyLeft", true);
        else animator.SetBool("KeyLeft", false);

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            animator.SetBool("KeyUp", false);
            animator.SetBool("KeyDown", false);
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            animator.SetBool("KeyRight", false);
            animator.SetBool("KeyLeft", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("RUN", true);
            currentSpeed = runSpeed;
        }
        else
        { 
            animator.SetBool("RUN",false);
            currentSpeed = walkSpeed;
        }

        // Manually trigger respawn 
        if (Input.GetKeyDown(PlayerConstants.RespawnKey))
        {
            ResetPlayerToOrigin();
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

        if (MovementVector == Vector3.zero)
        {
            currentSpeed = 0f;
        }

        Vector3 movement = new Vector3(MovementVector.x, 0f, MovementVector.y);
        characterController.Move(movement * currentSpeed * Time.fixedDeltaTime);
    }

    private bool IsPlayerBelowKillFloor()
    {
        return characterController.transform.position.y < killFloorHeight;
    }

    private void ResetPlayerToOrigin()
    {
        characterController.enabled = false;
        transform.position = playerResetPosition;
        characterController.enabled = true;
    }

    public static void RotateWithView(Vector3 vector,Transform cameraTransform)
	{
		Vector3 dir = cameraTransform.TransformDirection(vector);
        dir.Set(dir.x, 0, dir.z);
		vector = dir.normalized * vector.magnitude;
	}

    public void ResetAnimator()
    {
        animator.SetBool("KeyUp", false);
        animator.SetBool("KeyDown", false);
        animator.SetBool("KeyRight", false);
        animator.SetBool("KeyLeft", false);
    }
}
