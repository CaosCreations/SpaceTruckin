using UnityEngine;

public class ShipLaunch : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float launchSpeed = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float liftHeight = 10f;
    [SerializeField] private Transform hangarDoors;

    private bool isLaunching = false;
    private bool isLifted = false; 
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 currentVelocity;

    [SerializeField] private bool manualStart;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
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
            if (!isLifted)
            {
                Vector3 targetUpPosition = originalPosition + Vector3.up * liftHeight;
                transform.position = Vector3.SmoothDamp(transform.position, targetUpPosition, ref currentVelocity, 1f / launchSpeed);
                
                // Has the ship reached the lift height
                if (Vector3.Distance(transform.position, targetUpPosition) < 0.1f)
                {
                    isLifted = true;
                }
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                // Move towards the exit 
                Vector3 targetForwardPosition = targetPosition - hangarDoors.forward * 10f; // Adjust the distance as needed
                transform.position = Vector3.SmoothDamp(transform.position, targetForwardPosition, ref currentVelocity, 1f / acceleration);
            }

            if (isLifted && Quaternion.Angle(transform.rotation, targetRotation) < 1f && Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isLaunching = false;
            }
        }
    }

    public void Launch()
    {
        Vector3 exitDirection = (hangarDoors.position - transform.position).normalized;
        targetPosition = hangarDoors.position + exitDirection * 10f;
        targetRotation = Quaternion.LookRotation(exitDirection, Vector3.up);
        isLaunching = true;
    }
}
