using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    private float minScale, maxScale;

    // Todo: Coroutine with random duration to spawn the obstacles
    // Separate obstacle spawner script?

    private void Init()
    {
        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject newObstacle = Instantiate(obstaclePrefabs[index]);
        
        var range = GetPositionRange(newObstacle.transform.localScale);
        float yPosition = Random.Range(range.Item1, range.Item2);
        float xPosition = Random.Range(0f, 4f); // replace with screen bounds
        Vector2 obstaclePosition = new Vector2(xPosition, yPosition);

    }

    private System.ValueTuple<float, float> GetPositionRange(Vector2 scale)
    {
        var yRange = new System.ValueTuple<float, float>();
        // Todo: Calculate yMin and yMax based on the scale of the object 
        // and the screen bounds

        return yRange; 
    }
}

