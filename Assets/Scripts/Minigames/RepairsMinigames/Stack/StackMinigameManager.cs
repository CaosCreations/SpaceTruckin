using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackMinigameManager : MonoBehaviour
{
    [SerializeField] private int maxScore = 3;

    private CubeCornersPositionPile cubeCornersPositionPile = new CubeCornersPositionPile();

    [SerializeField] private GameObject cubePrefab;

    [SerializeField] private Transform cubeSpawnStartPosition;

    private CubeMover cubeMover;

    private StackMinigameUI stackMinigameUI;

    [SerializeField] private Button stackButton;

    [SerializeField] private Button replayButton;


    private List<GameObject> stackedCubes { get; set; } = new List<GameObject>(); 

    private bool gameRunning = false;

    private void Awake()
    {
        stackMinigameUI = GetComponentInChildren<StackMinigameUI>();

        cubeMover = GetComponentInChildren<CubeMover>();

        stackButton.onClick.RemoveAllListeners();
        stackButton.onClick.AddListener(DoPlayButton);

        replayButton.onClick.RemoveAllListeners();
        replayButton.onClick.AddListener(ResetGame);
    }

    private void Update()
    {
        if (gameRunning == false)
            return;

        cubeMover.MoveCube();
    }

    private void OnEnable()
    {
        ResetGame();
    }

    // When the player presses the play button, he or she attempts to stack the current moving cube on top of the cube below
    // If the cubes are stacked, the game goes on, and we spawn a new cube on top
    // If not it's game over
    public void DoPlayButton()
    {
        CubeCornersPositionTracker topCubeCornerPosition = cubeCornersPositionPile.CubeCornersPositionList[0];
        

        // There is only one cube at the beginning. The player can place it wherever. There is no cutting involved yet.
        if (cubeCornersPositionPile.CubeCornersPositionList.Count == 1)
        {

            SpawnTopCube(spawnPosition: cubeCornersPositionPile.CubeCornersPositionList[0].transform.position + new Vector3(0f, cubePrefab.transform.localScale.y, 0f),
                                                          cubeWidth: cubeCornersPositionPile.CubeCornersPositionList[0].transform.localScale.x);

            return;
        }

        float cubeOverlapDistance = CubesOverlapDistance(cubeCornersPositionPile);
        CubeCornersPositionTracker bottomCubeCornerPosition = cubeCornersPositionPile.CubeCornersPositionList[1];

        // Cubes aren't stacked. It's game over
        if (cubeOverlapDistance == 0f)
        {
            gameRunning = false;
            stackMinigameUI.SetGameUI(GameState.Lose);
            return;
        }

        // Cubes are stacked
        // we cut part of the top cube so that its side that
        // was sticking out is now aligned with that of the bottom cube
        CutStickingOutTopCubeSide(topCubeCornerPosition, bottomCubeCornerPosition);

        // Only spawn next top cube if the stack hasn't reached to top rank yet
        if (stackedCubes.Count >= maxScore)
        {
            gameRunning = false;
            stackMinigameUI.SetGameUI(GameState.Win);
            return;
        }

        SpawnTopCube(spawnPosition: bottomCubeCornerPosition.transform.position + new Vector3(0f, cubePrefab.transform.localScale.y, 0f),
                     cubeWidth: cubeOverlapDistance);
      
    }

    private void CutStickingOutTopCubeSide(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition)
    {
        float XspawnPosition;

        float width = CubesOverlapDistance(cubeCornersPositionPile);

        // Top cube to the left of bottom cube
        if (bottomCubeCornerPosition.GetLeftCornerPosition() < topCubeCornerPosition.GetLeftCornerPosition())
        {
            XspawnPosition = topCubeCornerPosition.GetLeftCornerPosition() + width / 2;
        }

        // Top cube to the right of bottom cube
        else
        {
            XspawnPosition = bottomCubeCornerPosition.GetLeftCornerPosition() + width / 2;
        }

        // Cutting off the current top cube (moving and resizing it)

        bottomCubeCornerPosition.transform.position = new Vector3(XspawnPosition, bottomCubeCornerPosition.transform.position.y, bottomCubeCornerPosition.transform.position.z);

        bottomCubeCornerPosition.transform.localScale = new Vector3(width, bottomCubeCornerPosition.transform.localScale.y, bottomCubeCornerPosition.transform.localScale.z);
    }

    private float CubesOverlapDistance(CubeCornersPositionPile pile)
    {
            float topCubeLeftCornerXposition = pile.CubeCornersPositionList[0].GetLeftCornerPosition();
            float topCubeRightCornerXposition = pile.CubeCornersPositionList[0].GetRightCornerPosition();
            float bottomCubeLeftCornerXposition = pile.CubeCornersPositionList[1].GetLeftCornerPosition();
            float bottomCubeRightCornerXposition = pile.CubeCornersPositionList[1].GetRightCornerPosition();

            // If the corners are this far apart, it can only mean that top and bottom cubes are not stacked on top of each other,
            // so there is no overlap
            if (topCubeLeftCornerXposition > bottomCubeRightCornerXposition ||
                topCubeRightCornerXposition < bottomCubeLeftCornerXposition)
            {
                return 0;
            }

            // The cubes are stacked, so there is some overlap
            else
            {
                float cubeOverlap = pile.CubeCornersPositionList[1].transform.localScale.x
                                    - Mathf.Abs(bottomCubeLeftCornerXposition - topCubeLeftCornerXposition);
                return cubeOverlap;
            }
    }

    private void SpawnTopCube(Vector3 spawnPosition, float cubeWidth)
    {
        GameObject topcube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

        topcube.transform.localScale = new Vector3(cubeWidth, topcube.transform.localScale.y, topcube.transform.localScale.z);

        cubeMover.CurrentMovingCube = topcube.transform;
        stackedCubes.Add(topcube);
        cubeCornersPositionPile.Add(topcube.GetComponent<CubeCornersPositionTracker>());
    }

    public void ResetGame()
    {
        foreach(GameObject cube in stackedCubes)
        {
            Destroy(cube);
        }

        stackedCubes.Clear();

        cubeCornersPositionPile.ResetPile();


        SpawnTopCube(cubeSpawnStartPosition.position, cubePrefab.transform.localScale.x);


        gameRunning = true;

        stackMinigameUI.SetGameUI(GameState.NewGame);
    }
}
