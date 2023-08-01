using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StackMiniGame_GameplayManager : MonoBehaviour
{
    [Header("Dependencies")]
    private ICubeSpawner cubeSpawner;

    [SerializeField] private CubeMover cubeMover;

    [SerializeField] private ScreenShaker screenShaker;

    [SerializeField] private CubeStack cubeStack;

    [SerializeField] private GameState gameStates;

    [Header("Gameplay")]

    [Range(0.0f, 2f)]
    [Tooltip("...")]
    [SerializeField] private float stackFreezeSeconds;

    [Range(0.0f, 2f)]
    [Tooltip("...")]
    [SerializeField] private float screenShakeSeconds;

    [SerializeField] private int fullWinScore;

    [SerializeField] private int partialWinScore;

    public GameState GameStates => gameStates;

    public event UnityAction GameResetEvent;

    public event UnityAction GameEndEvent;

    private IEnumerator stackCoroutine;

    private void Awake()
    {
        cubeSpawner = GetComponentInChildren<ICubeSpawner>();
    }

    private void Start()
    {
        ResetGame();
    }

    // When the player presses the play button, he or she attempts to stack the current moving cube on top of the cube below
    // If the cubes are stacked, the game goes on, and we spawn a new cube on top
    // If not it's game over

    public void StackCube()
    {
        if (stackCoroutine != null)
            return;

        stackCoroutine = StackCubeCoroutine();
        StartCoroutine(stackCoroutine);
    }

    private IEnumerator StackCubeCoroutine()
    {
        CubeCornersPositionTracker bottomCube = cubeStack.CubeCornersPositionPile.BottomCube;

        // Start coroutine based on how many cubes 
        if (cubeStack.CubeCornersPositionPile.TopCube == null)
        {
            yield return StartCoroutine(PlaceFirstCube(bottomCube));
        }
        else
        {
            yield return StartCoroutine(StackCubes(bottomCube));
        }
    }

    private IEnumerator PlaceFirstCube(CubeCornersPositionTracker bottomCube)
    {
        screenShaker.ShakeScreen(seconds: screenShakeSeconds);
        yield return StartCoroutine(cubeMover.FreezeCubeMovement(stackFreezeSeconds));

        cubeSpawner.SpawnStackedCube(spawnPosition: bottomCube.transform.position,
                                     setWidth: bottomCube.transform.localScale.x);

        stackCoroutine = null;
    }

    private IEnumerator StackCubes(CubeCornersPositionTracker bottomCube)
    {
        CubeCornersPositionTracker topCube = cubeStack.CubeCornersPositionPile.TopCube;

        CubeOverlap cubeOverlap = cubeStack.GetCubeOverlap();

        // Start coroutine based on how the cubes are overlapping
        if (cubeOverlap == CubeOverlap.None)
        {
            yield return StartCoroutine(GameOverCoroutine());
        }
        else
        {
            cubeSpawner.CutCube(topCube, bottomCube, cubeOverlap);

            yield return StartCoroutine(StackCubeCoroutineAfterCut(topCube));
        }
    }

    private IEnumerator GameOverCoroutine()
    {
        if (cubeStack.StackedCubes.Count >= partialWinScore + 1)
            gameStates.SetCurrentState("partial win");
        else
            gameStates.SetCurrentState("lose");

        GameEndEvent?.Invoke();
        stackCoroutine = null;

        yield break;
    }

    private IEnumerator StackCubeCoroutineAfterCut(CubeCornersPositionTracker topCube)
    {
        screenShaker.ShakeScreen(seconds: screenShakeSeconds);
        yield return StartCoroutine(cubeMover.FreezeCubeMovement(stackFreezeSeconds));

        if (cubeStack.StackedCubes.Count >= fullWinScore)
        {
            gameStates.SetCurrentState("full win");
            GameEndEvent?.Invoke();
            stackCoroutine = null;
            yield break;
        }

        cubeSpawner.SpawnStackedCube(spawnPosition: topCube.transform.position, setWidth: topCube.transform.localScale.x);
        stackCoroutine = null;
    }

    public void ResetGame()
    {
        gameStates.SetCurrentState("new game");

        GameResetEvent?.Invoke();

        cubeSpawner.SpawnBottomCube();
    }
}