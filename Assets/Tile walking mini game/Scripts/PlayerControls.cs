using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private RectTransform playerRectTransform;

    [SerializeField] private TileWalkingUI tileWalkingUI;

    private Vector3 playerStartPosition;

    private int playerXGridPosition = 0;

    private int playerYGridPosition = 0;

    private bool canMove = true;

    private int[] defaultInput = new int[] {0, 0};

    private int[] upInput = new int[] { 0, 1 };
    private int[] downInput = new int[] { 0, -1 };
    private int[] leftInput = new int[] { -1, 0 };
    private int[] rightInput = new int[] { 1, 0 };

    private void Awake()
    {
        playerStartPosition = playerRectTransform.localPosition;
        gridManager.WinEvent += DisablePlayerMovement;
        gridManager.LoseEvent += DisablePlayerMovement;
    }

    private void Update()
    {
        if (!canMove)
            return;

        int[] playerInput = GetPlayerInput();

        if (playerInput == defaultInput)
            return;

        MovePlayerToTile(Xmovement: playerInput[0], Ymovement: playerInput[1]);
    }

    private int[] GetPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W))
            return upInput;

        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A))
            return leftInput;

        else if (Input.GetKeyDown(KeyCode.S))
            return downInput;

        else if (Input.GetKeyDown(KeyCode.D))
            return rightInput;

        return defaultInput;
    }

    private bool CheckIfTileIsWalkable(int XInput, int YInput)
    {
        Tile desiredTile = gridManager.GetTileAt(playerXGridPosition + XInput, playerYGridPosition + YInput);

        if (desiredTile != null && desiredTile.TileStatus != TileStatus.Obstacle)
        {
            return true;
        }

        return false;
    }

    // The player moves on the grid one tile at a time. Left, right, up or down.
    // We use the grid's X and Y axis to do so

    private void MovePlayerToTile(int Xmovement, int Ymovement)
    {
        if (CheckIfTileIsWalkable(XInput: Xmovement, YInput: Ymovement))
        {
            playerXGridPosition += Xmovement;
            playerYGridPosition += Ymovement;

            playerRectTransform.position = gridManager.GetTileAt(playerXGridPosition, playerYGridPosition).RectTransform.position;
            gridManager.UpdateTileStatus(playerXGridPosition, playerYGridPosition);
        }  
    }

    private void DisablePlayerMovement()
    {
        canMove = false;
    }

    public void ResetPlayerMovement()
    {
        playerRectTransform.localPosition = playerStartPosition;
        playerXGridPosition = 0;
        playerYGridPosition = 0;
        canMove = true;
    }
}
