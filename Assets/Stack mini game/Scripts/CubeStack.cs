using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CubeStack : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;

    [SerializeField] private StackMiniGame_GameplayManager gameplayManager;

    public CubeCornersPositionPile CubeCornersPositionPile { get; private set; } = new CubeCornersPositionPile();

    public List<GameObject> StackedCubes { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        cubeSpawner.CubeSpawnedEvent += AddTopCubeDataToPile;

        gameplayManager.GameResetEvent += Reset;
    }

    public CubeOverlap GetCubeOverlap()
    {
        float topCubeLeftCornerXposition = CubeCornersPositionPile.TopCube.GetLeftCornerPosition();
        float topCubeRightCornerXposition = CubeCornersPositionPile.TopCube.GetRightCornerPosition();
        float bottomCubeLeftCornerXposition = CubeCornersPositionPile.BottomCube.GetLeftCornerPosition();
        float bottomCubeRightCornerXposition = CubeCornersPositionPile.BottomCube.GetRightCornerPosition();

        // If the corners are this far apart, it can only mean that top and bottom cubes are not stacked on top of each other,
        // so there is no overlap
        if ((topCubeRightCornerXposition < bottomCubeLeftCornerXposition) ||
             topCubeLeftCornerXposition > bottomCubeRightCornerXposition)
            return CubeOverlap.None;

        if(bottomCubeRightCornerXposition < topCubeRightCornerXposition )
            return CubeOverlap.Right;

        return CubeOverlap.Left;
    }

    private void AddTopCubeDataToPile(GameObject cube)
    {
        StackedCubes.Add(cube);
        CubeCornersPositionPile.Add(cube.GetComponent<CubeCornersPositionTracker>());
    }

    private void Reset()
    {
        foreach (GameObject cube in StackedCubes)
        {
            cube.SetActive(false);
        }

        StackedCubes.Clear();

        CubeCornersPositionPile.ResetPile();
    }
}

public enum CubeOverlap
{
    None,
    Left,
    Right
}
