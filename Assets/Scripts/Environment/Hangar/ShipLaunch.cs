using UnityEngine;

public class ShipLaunch : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float liftSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float launchSpeed = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float liftHeight = 5f;
    private Transform hangarDoors;

    private bool isLaunching = false;
    private bool hasLifted = false;
    private bool isRotating = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 liftVelocity;
    private Vector3 currentVelocity;

    [SerializeField] private bool manualStart;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        hangarDoors = GameObject.FindGameObjectWithTag("HangarDoors").transform;
    }

    private void Update()
    {
        if (manualStart)
        {
            transform.SetPositionAndRotation(originalPosition, originalRotation);
            Launch();
            manualStart = false;
        }

        if (isLaunching)
        {
            if (!hasLifted)
            {
                Vector3 targetUpPosition = originalPosition + Vector3.up * liftHeight;
                transform.position = Vector3.SmoothDamp(transform.position, targetUpPosition, ref liftVelocity, 1f / liftSpeed, acceleration);

                // Check if the ship has reached the lift height
                if (Vector3.Distance(transform.position, targetUpPosition) < 0.1f)
                {
                    hasLifted = true;
                }
            }
            else if (!isRotating)
            {
                isRotating = true;
                targetRotation = Quaternion.LookRotation(hangarDoors.position - transform.position, Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Finish rotation before moving towards exit 
                if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / launchSpeed);
                }
            }

            if (isRotating && Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isLaunching = false;
                Destroy(gameObject);
            }
        }
    }

    public void Launch()
    {
        Vector3 exitDirection = (hangarDoors.position - transform.position).normalized;
        targetPosition = hangarDoors.position + exitDirection * 10f;
        isLaunching = true;
        hasLifted = false;
        isRotating = false;
    }
}
