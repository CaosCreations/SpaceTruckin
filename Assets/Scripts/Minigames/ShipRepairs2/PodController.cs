using UnityEngine;

public class PodController : MonoBehaviour
{
    private float yMin, yMax;
    private float midpointY = 0f;
    private float screenBorder = 2f;
    private float flightSpeed = 3.5f;
    private float fallSpeed = 1f;

    public Vector3 Position 
    {
        get => transform.position; set => transform.position = value;
    }

    private void Start()
    {
        yMin = -Camera.main.orthographicSize + screenBorder;
        yMax = Camera.main.orthographicSize/* - screenBorder*/;
        Debug.Log("Y min: " + yMin);
        Debug.Log("Y max: " + yMax);
    }

    private void Update()
    {
        flightSpeed = IsAboveMidpoint() ? flightSpeed : flightSpeed * -1;
        Debug.Log("Flight speed = " + flightSpeed);

        float newY;
        if (Input.GetMouseButton(0))
        {
            newY = Mathf.Clamp(
                Position.y + flightSpeed * Time.deltaTime, yMin, yMax);
        }
        else
        {
            newY = Mathf.Clamp(
                Position.y - fallSpeed * Time.deltaTime, yMin, yMax);
        }
        Position = new Vector3(Position.x, newY, Position.z);
    }

    private bool IsAboveMidpoint() => Position.y > midpointY;
}
