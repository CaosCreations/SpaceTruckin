using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICubeSpawner
{
    void SpawnBottomCube();
    void SpawnStackedCube(Vector3 spawnPosition, float setWidth);
    void CutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap);
}

public class CubeSpawner : MonoBehaviour, ICubeSpawner
{
    [SerializeField] private GameObject cubePrefab;

    [SerializeField] private Transform cubeSpawnStartPosition;

    public UnityAction<GameObject> CubeSpawnedEvent;

    [SerializeField]
    [Tooltip("...")]
    [Range(2, 20)]
    private int cubeDivisions = 3;

    private float cubeDivisionWidth;
    private readonly List<GameObject> cubePool = new List<GameObject>();

    private void Awake()
    {
        cubeDivisionWidth = cubePrefab.transform.localScale.x / cubeDivisions;
    }

    public void SpawnBottomCube()
    {
        GameObject spawnedCube = GetCubeFromPool();

        spawnedCube.transform.SetPositionAndRotation(cubeSpawnStartPosition.position, Quaternion.identity);
        spawnedCube.transform.localScale = new Vector3(cubePrefab.transform.localScale.x, spawnedCube.transform.localScale.y, spawnedCube.transform.localScale.z);

        CubeSpawnedEvent.Invoke(spawnedCube);
    }

    public void SpawnStackedCube(Vector3 spawnPosition, float setWidth)
    {
        spawnPosition += new Vector3(0f, cubePrefab.transform.localScale.y, 0f);

        GameObject spawnedCube = GetCubeFromPool();

        spawnedCube.transform.position = spawnPosition;
        spawnedCube.transform.rotation = Quaternion.identity;
        spawnedCube.transform.localScale = new Vector3(setWidth, spawnedCube.transform.localScale.y, spawnedCube.transform.localScale.z);

        CubeSpawnedEvent.Invoke(spawnedCube);
    }

    /// <summary>
    /// The cubes are divided in several parts of equal width, and cuts include a set number of complete parts.
    /// Following that logic, the more the cube divisions, the harder the game, because it is harder to get the timing right to stack smaller parts.
    /// On the one hand, it becomes easy for the player to do perfect stacks, on the other hand it sets a minimum width for the cube,
    /// making it easy to stack it.
    /// </summary>
    public void CutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        float topCornerXPosition = GetTopCornerPosition(topCubeCornerPosition, cubeOverlap);

        float bottomCornerXPosition = GetBottomCornerPosition(bottomCubeCornerPosition, cubeOverlap);

        float roundedCutWidth = GetRoundedCutWidth(topCornerXPosition, bottomCornerXPosition);

        ResizeCutCube(topCubeCornerPosition, cubeOverlap, bottomCornerXPosition, roundedCutWidth);
    }

    private float GetTopCornerPosition(CubeCornersPositionTracker topCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        return cubeOverlap == CubeOverlap.Right ? topCubeCornerPosition.GetLeftCornerPosition() : topCubeCornerPosition.GetRightCornerPosition();
    }

    private float GetBottomCornerPosition(CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        return cubeOverlap == CubeOverlap.Right ? bottomCubeCornerPosition.GetRightCornerPosition() : bottomCubeCornerPosition.GetLeftCornerPosition();
    }

    private float GetRoundedCutWidth(float topCornerX, float bottomCornerX)
    {
        float distanceBetweenCubes = Mathf.Abs(topCornerX - bottomCornerX);

        // Calculate number of cube divisions needed to cover the distance
        int numDivisions = Mathf.CeilToInt(distanceBetweenCubes / cubeDivisionWidth);

        float roundedCutWidth = numDivisions * cubeDivisionWidth;
        return roundedCutWidth;
    }

    private void ResizeCutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeOverlap cubeOverlap, float bottomCornerXPosition, float roundedCutWidth)
    {
        float newXCubePosition = CalculateNewXCubePosition(bottomCornerXPosition, roundedCutWidth, cubeOverlap);
        Vector3 newScale = new Vector3(roundedCutWidth, topCubeCornerPosition.transform.localScale.y, topCubeCornerPosition.transform.localScale.z);

        topCubeCornerPosition.transform.SetPositionAndRotation(
            new Vector3(newXCubePosition, topCubeCornerPosition.transform.position.y, topCubeCornerPosition.transform.position.z), Quaternion.identity);

        topCubeCornerPosition.transform.localScale = newScale;
    }

    private float CalculateNewXCubePosition(float bottomCornerXPosition, float roundedCutWidth, CubeOverlap cubeOverlap)
    {
        // Use offset based on overlap direction to calculate x value 
        float offset = cubeOverlap == CubeOverlap.Right ? -1f : 1f;
        return bottomCornerXPosition + offset * (roundedCutWidth / 2f);
    }

    private GameObject GetCubeFromPool()
    {
        GameObject cube = cubePool.Find(c => !c.activeSelf);

        if (cube == null)
        {
            cube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity, transform);
            cubePool.Add(cube);
        }

        cube.SetActive(true);

        return cube;
    }
}
