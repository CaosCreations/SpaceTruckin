using UnityEngine;

public class PodController : MonoBehaviour
{
    private float yMin, yMax;
    private float screenBorder = 2f;
    private float flightSpeed = 2f;
    private float fallSpeed = 1f;

    private void Start()
    {
        yMin = -Camera.main.orthographicSize/* + screenBorder*/;
        yMax = Camera.main.orthographicSize - screenBorder;
        Debug.Log("Y min: " + yMin);
        Debug.Log("Y max: " + yMax);
    }

    private void Update()
    {
        float newY;
        if (Input.GetMouseButton(0))
        {
            newY = Mathf.Clamp(transform.position.y + flightSpeed * Time.deltaTime, yMin, yMax);
        }
        else
        {
            newY = Mathf.Clamp(transform.position.y - fallSpeed * Time.deltaTime, yMin, yMax);
        }
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
