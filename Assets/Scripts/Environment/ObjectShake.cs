using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    [Header("Shake settings")]
    [Range(0f, 1f)] public float ShakeIntensity = 0.1f;
    [Range(0f, 1f)] public float ShakeRandomness = 0.1f;

    private Vector3 originalPosition;
    private bool isShaking;

    private void Start()
    {
        StartShaking();
    }

    private void Update()
    {
        if (isShaking)
        {
            Vector3 newPosition = originalPosition + ShakeIntensity * ShakeRandomness * Random.insideUnitSphere;
            transform.localPosition = newPosition;
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
