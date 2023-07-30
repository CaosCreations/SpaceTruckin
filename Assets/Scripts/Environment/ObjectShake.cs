using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    [Header("Shake settings")]
    [Range(0f, 1f)] public float ShakeIntensity = 0.1f;
    [Range(0f, 1f)] public float ShakeRandomness = 0.1f;
    [Range(0f, 1f)] public float ShakeSmoothening;

    public Vector3 originalPosition;
    private Vector3 shakeVelocity;
    private bool isShaking = false;
    [SerializeField] private bool start;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    private void Start()
    {
        StartShaking();
    }

    private void Update()
    {
        if (start)
        {
            StartShaking();
            start = false;
        }

        if (isShaking)
        {
            Vector3 targetPosition = originalPosition + ShakeIntensity * ShakeRandomness * Random.insideUnitSphere;
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref shakeVelocity, ShakeSmoothening);
        }
    }

    public void StartShaking()
    {
        isShaking = true;
        originalPosition = transform.localPosition;
    }

    public void StopShaking()
    {
        isShaking = false;
        transform.localPosition = originalPosition;
    }
}
