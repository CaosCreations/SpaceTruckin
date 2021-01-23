using UnityEngine;

public class PodController : MonoBehaviour
{
    private float yMin = -4f;
    private float yMax = 4f;
    private float fallSpeed = 1f;

    private void Update()
    {
        float newY = Mathf.Clamp(transform.position.y - fallSpeed * Time.deltaTime, yMin, yMax);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

    }
}
