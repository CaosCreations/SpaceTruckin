using UnityEngine;

public class PodController : MonoBehaviour
{
    private float yMin, yMax;
    private float midpointY = 0f;
    private float screenBorder = 2f;

    private float defaultFlightSpeed = 3.5f;
    private float currentFlightSpeed;
    private float defaultFallSpeed = 1f;
    private float currentFallSpeed;

    private Direction direction; 

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
        currentFlightSpeed = defaultFlightSpeed;
        direction = Direction.Up;
    }

    private void Update()
    {
        Debug.Log("Flight speed = " + currentFlightSpeed);

        DeterminePosition();
    }

    private void DeterminePosition()
    {
        DetermineDirection();
        DetermineSpeed();

        float newY;
        if (Input.GetMouseButton(0))
        {
            newY = Mathf.Clamp(
                Position.y + currentFlightSpeed * Time.deltaTime, yMin, yMax);
        }
        else
        {
            newY = Mathf.Clamp(
                Position.y - currentFallSpeed * Time.deltaTime, yMin, yMax);
        }
        Position = new Vector3(Position.x, newY, Position.z);
    }

    private void DetermineDirection()
    {
        if (IsAboveMidpoint())
        {
            direction = Direction.Up;
        }
        else
        {
            direction = Direction.Down;
        }
    }

    private bool IsAboveMidpoint() => Position.y > midpointY;

    private void  DetermineSpeed()
    {
        switch (direction)
        {
            case Direction.Up:
                currentFlightSpeed = defaultFlightSpeed;
                currentFallSpeed = defaultFallSpeed;
                break;
            case Direction.Down:
                currentFlightSpeed = defaultFlightSpeed * -1f;
                currentFallSpeed = defaultFallSpeed * -1f;
                break;
        }
    }


}
