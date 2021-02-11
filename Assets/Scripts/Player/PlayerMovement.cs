using UnityEngine;

public enum Direction
{
    Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight
}

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 movementVector;
    public float killFloorHeight = -25;
    public Vector3 playerResetPosition = new Vector3(8.5f, 0.8f, -10f);

    [SerializeField] private Animator animator;

    private float currentSpeed; 
    [SerializeField] private float maximumSpeed; 
    [SerializeField] private float acceleration;

    private CharacterController characterController;
    private float gravity = -9.81f;




    //player movement relate to camera
    public Transform CameraTransform;






    private void Start()
    {
        characterController = GetComponent<CharacterController>();


        // initialize the player's Camera
		CameraTransform = Camera.main.transform;



    }

    // Get input in Update 
    private void Update()
    {

        if (PlayerManager.Instance.isPaused)
        {
            return;
        }
        if (IsPlayerBelowKillFloor())
        {
            ResetPlayerToOrigin();
            return;

        }

        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");


        SetDirection();

        RotateWithView(movementVector,CameraTransform);



    }

    // Move player in FixedUpdate 
    private void FixedUpdate()
    {
        if (PlayerManager.Instance.isPaused)
        {
            return;
        }

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
        if (movementVector == Vector3.up)
        {
            animator.SetBool("Up", true);
        }
        else if (movementVector == new Vector3(-1f, 1f))
        {
            animator.SetBool("Up", true);
        }
        else if (movementVector == Vector3.left)
        {
            animator.SetBool("Left", true);
        }
        else if (movementVector == new Vector3(-1f, -1f))
        {
            animator.SetBool("Left", true);
        }
        else if (movementVector == Vector3.down)
        {
            animator.SetBool("Down", true);
        }
        else if (movementVector == new Vector3(1f, -1f))
        {
            animator.SetBool("Down", true);
        }
        else if (movementVector == Vector3.right)
        {
            animator.SetBool("Right", true);
        }
        else if (movementVector == Vector3.one)
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







        			 
			   if (Input.GetKey(KeyCode.W)){ animator.SetBool("KeyUp",true);  }
			      if (!Input.GetKey(KeyCode.W)){ animator.SetBool("KeyUp",false);  }

				     if (Input.GetKey(KeyCode.S)){ animator.SetBool("KeyDown",true);    }
					 if (!Input.GetKey(KeyCode.S)){ animator.SetBool("KeyDown",false);  }

					 

			   if (Input.GetKey(KeyCode.D)){ animator.SetBool("KeyRight",true);  }
			      if (!Input.GetKey(KeyCode.D)){ animator.SetBool("KeyRight",false);  }

				  

				     if (Input.GetKey(KeyCode.A)){ animator.SetBool("KeyLeft",true);  }
					 if (!Input.GetKey(KeyCode.A)){ animator.SetBool("KeyLeft",false);  }


                      if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
                      {
                          animator.SetBool("KeyUp",false);
                          animator.SetBool("KeyDown",false);

                      }

                      if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                      {
                          animator.SetBool("KeyRight",false);
                          animator.SetBool("KeyLeft",false);

                      }



                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                         animator.SetBool("RUN",true);
                         currentSpeed=7;
                           }
					if (!Input.GetKey(KeyCode.LeftShift))
                    { 
                        
                        animator.SetBool("RUN",false);
                        currentSpeed=3;
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

        if (movementVector == Vector3.zero)
        {
            currentSpeed = 0f;
        }

        Vector3 movement = new Vector3(movementVector.x, 0f, movementVector.y);
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
		
		
		Vector3 dir =cameraTransform.TransformDirection(vector);
        dir.Set(dir.x, 0, dir.z);
		vector = dir.normalized * vector.magnitude;
	}






}
