using UnityEngine;

public enum Direction {
    Up,
    UpLeft,
    Left,
    DownLeft,
    Down,
    DownRight,
    Right,
    UpRight
}

public class PlayerMovement : MonoBehaviour {
    public static Vector3 movementVector;
    public float killFloorHeight = -25;
    public Vector3 playerResetPosition = new Vector3 (8.5f, 0.8f, -10f);

    [SerializeField] private Animator animator;

    private float currentSpeed;
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float acceleration;

    private CharacterController characterController;
    private float gravity = -9.81f;

    //player movement relate to camera
    public Transform CameraTransform;

    //RayCast To cam stuff
    public float myRayDistance;
    public GameObject mypointToRay;
    public GameObject wallToTurnInvisible;
    public float wallVisibity;
    public float wallHitDistace;
    public Material transparentMat;
     public Material opaqueStd;



    private void Start () {
        characterController = GetComponent<CharacterController> ();

        // initialize the player's Camera
        CameraTransform = Camera.main.transform;
        

    }

    // Get input in Update 
    private void Update () 
    {
        CameraTransform = Camera.main.transform;

        if (PlayerManager.Instance.isPaused) {
            return;
        }
        if (IsPlayerBelowKillFloor ()) {
            ResetPlayerToOrigin ();
            return;

        }

        
        Quaternion inputRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(CameraTransform.forward,Vector3.up),Vector3.up);
			
            

        movementVector.x = Input.GetAxisRaw ("Horizontal");
        movementVector.z = Input.GetAxisRaw ("Vertical");
      
        

        SetDirection ();

        //to rotate player move with the cam
          movementVector = inputRotation * movementVector;


        //raycast for wall transparency

        myRayDistance = Vector3.Distance (mypointToRay.transform.position, transform.position);

        var dir = mypointToRay.transform.position - transform.position;
        var ray = new Ray (transform.position, dir.normalized);

        RaycastHit hitr;
        bool Raycastright = Physics.Raycast (ray, out hitr);

        if (Physics.Raycast (ray, myRayDistance)) {

            Debug.Log ("ASODKASOD");
            wallHitDistace=hitr.distance;


            if(wallHitDistace>=4){wallHitDistace=4;}





            if (hitr.transform.gameObject.tag == "TurnTransparent") 
            {
                wallToTurnInvisible = hitr.transform.gameObject;
                var wallRenderee = wallToTurnInvisible.GetComponent<Renderer> ();
                wallVisibity = wallRenderee.material.color.a;

                if (wallVisibity >= 0.1 && wallVisibity <= 1) {
                    //change material from cutout to fade
                    wallRenderee.material= transparentMat;
                  



                    wallRenderee.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f-wallHitDistace/5);
                    //U CAN MESS WITH WALL ALPHA WITH HITR.DISTANCE


                    




                    




                } else {
                    wallVisibity = 1;
                    wallToTurnInvisible=null;
                }

            }

        } else {
            var wallRenderee = wallToTurnInvisible.GetComponent<Renderer> ();
            wallRenderee.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);

             wallRenderee.material= opaqueStd;
             
        }
        Debug.DrawLine (transform.position, mypointToRay.transform.position, Color.yellow);

        //raycast for wall transparencyEND

    }

    // Move player in FixedUpdate 
    private void FixedUpdate () {
        if (PlayerManager.Instance.isPaused) {
            return;
        }

        // Adjust for diagonal input 
        if (movementVector.magnitude > 1f) {
            movementVector /= movementVector.magnitude;
        }



        ApplyGravity ();
        MovePlayer ();

    }

    private void SetDirection () 
    {
        //I've changed from vecto2 to 3 but it's weird

        if (movementVector == Vector3.up) {
            animator.SetBool ("Up", true);
        } else if (movementVector == new Vector3 (-1f, 0,-1f)) {
            animator.SetBool ("Up", true);
        } else if (movementVector == Vector3.left) {
            animator.SetBool ("Left", true);
        } else if (movementVector == new Vector3 (-1f, 0,-1f)) {
            animator.SetBool ("Left", true);
        } else if (movementVector == Vector3.down) {
            animator.SetBool ("Down", true);
        } else if (movementVector == new Vector3 (1f, 0,1f)) {
            animator.SetBool ("Down", true);
        } else if (movementVector == Vector3.right) {
            animator.SetBool ("Right", true);
        } else if (movementVector == Vector3.one) {
            animator.SetBool ("Right", true);

        } else {

            animator.SetBool ("Up", false);
            animator.SetBool ("Down", false);
            animator.SetBool ("Right", false);
            animator.SetBool ("Left", false);
        }

        if (Input.GetKey (KeyCode.W)) { animator.SetBool ("KeyUp", true); }
        if (!Input.GetKey (KeyCode.W)) { animator.SetBool ("KeyUp", false); }

        if (Input.GetKey (KeyCode.S)) { animator.SetBool ("KeyDown", true); }
        if (!Input.GetKey (KeyCode.S)) { animator.SetBool ("KeyDown", false); }

        if (Input.GetKey (KeyCode.D)) { animator.SetBool ("KeyRight", true); }
        if (!Input.GetKey (KeyCode.D)) { animator.SetBool ("KeyRight", false); }

        if (Input.GetKey (KeyCode.A)) { animator.SetBool ("KeyLeft", true); }
        if (!Input.GetKey (KeyCode.A)) { animator.SetBool ("KeyLeft", false); }

        if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S)) {
            animator.SetBool ("KeyUp", false);
            animator.SetBool ("KeyDown", false);

        }

        if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D)) {
            animator.SetBool ("KeyRight", false);
            animator.SetBool ("KeyLeft", false);

        }

        if (Input.GetKey (KeyCode.LeftShift)) {
            animator.SetBool ("RUN", true);
            currentSpeed = 7;
        }
        if (!Input.GetKey (KeyCode.LeftShift)) {

            animator.SetBool ("RUN", false);
            currentSpeed = 3;
        }

    }

    private void ApplyGravity () {
        characterController.Move (new Vector3 (0, gravity * Time.fixedDeltaTime, 0));
    }

    private void MovePlayer () {
        if (currentSpeed < maximumSpeed) {
            currentSpeed += acceleration;
        }

        if (movementVector == Vector3.zero) {
            currentSpeed = 0f;
        }

        Vector3 movement = new Vector3 (movementVector.x, 0f, movementVector.z);
        characterController.Move (movement * currentSpeed * Time.fixedDeltaTime);
    }

    private bool IsPlayerBelowKillFloor () {
        return characterController.transform.position.y < killFloorHeight;
    }

    private void ResetPlayerToOrigin () {
        characterController.enabled = false;
        transform.position = playerResetPosition;
        characterController.enabled = true;
    }

}