using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float frameRate = 0.15f;

    // The sprites to loop through when moving  
    public Sprite[] UpFrames;
    public Sprite[] LeftFrames;
    public Sprite[] DownFrames;
    public Sprite[] RightFrames;

    // Stores the currently looping sprites 
    private Sprite[] frameArray;

    // The sprites displayed when stationary  
    // Indices are: 0-3 WASD
    public Sprite[] StationaryFrames;

    public Material Material;

    // Stores the current stationary sprite
    private Sprite stationaryPosition;

    private int currentFrame;
    private float timer;
    private bool flipped;

    public static Direction Direction;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Default stationary position is facing down 
        stationaryPosition = StationaryFrames[2];
        Material.mainTexture = stationaryPosition.texture;

        // Set frame rate proportionally to movement speed 
        // Enable this when not Serialized
        // And set movementSpeed to static member*
        //frameRate = PlayerMovement.movementSpeed / 1000; 

    }

    private void FixedUpdate()
    {
        if (PlayerMovement.MovementVector == Vector3.zero)
        {
            Material.mainTexture = stationaryPosition.texture;
        }
        else
        {
            UpdateFrameArray();

            timer += Time.fixedDeltaTime;

            // Do something every "frame",
            // according to the frameRate defined above 
            if (timer >= frameRate)
            {
                timer -= frameRate;

                // Reset currentFrame to 0 when it reaches the length of the array 
                currentFrame = (currentFrame + 1) % frameArray.Length;

                // Flip sprite if moving left 
                //spriteRenderer.flipX = flipped ? true : false;
                Material.mainTextureScale = flipped ? new Vector2(-1f, 1f) : Vector2.one;

                // Update the sprite being rendered
                //spriteRenderer.sprite = frameArray[currentFrame]; 
                Material.mainTexture = frameArray[currentFrame].texture;
            }
        }
    }

    // Assign frameArray to the frames corresponding to 
    // the player's current direction 
    private void UpdateFrameArray()
    {
        // Ugliest switch block in existence 
        switch (Direction)
        {
            case Direction.Up:
                frameArray = UpFrames;
                stationaryPosition = StationaryFrames[0];
                flipped = false;
                break;
            case Direction.UpLeft:
                frameArray = LeftFrames;
                stationaryPosition = StationaryFrames[1];
                flipped = false;
                break;
            case Direction.Left:
                frameArray = LeftFrames;
                stationaryPosition = StationaryFrames[1];
                flipped = false;
                break;
            case Direction.DownLeft:
                frameArray = LeftFrames;
                stationaryPosition = StationaryFrames[1];
                flipped = false;
                break;
            case Direction.Down:
                frameArray = DownFrames;
                stationaryPosition = StationaryFrames[2];
                flipped = false;
                break;
            case Direction.DownRight:
                frameArray = RightFrames;
                stationaryPosition = StationaryFrames[3];
                flipped = false;
                break;
            case Direction.Right:
                frameArray = RightFrames;
                stationaryPosition = StationaryFrames[3];
                flipped = false;
                break;
            case Direction.UpRight:
                frameArray = RightFrames;
                stationaryPosition = StationaryFrames[3];
                flipped = false;
                break;
        }
    }
}
