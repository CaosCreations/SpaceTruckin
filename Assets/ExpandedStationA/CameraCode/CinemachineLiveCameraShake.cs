using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineLiveCameraShake : CinemachineLiveCameraBehaviour
{
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    private Coroutine shakeCoroutine;

    private void SetPerlinNoise()
    {
        if (virtualCamera != null)
        {
            perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            // For some reason getting errors here only in the built game 
            if (perlinNoise == null)
            {
                Debug.LogWarning(nameof(CinemachineBasicMultiChannelPerlin) + " component not found on " + nameof(CinemachineLiveCameraShake));
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        SetPerlinNoise();
    }

    protected override void UpdateActiveCamera()
    {
        base.UpdateActiveCamera();
        SetPerlinNoise();
    }

    public void Shake(CameraShakeSettings settings)
    {
        UpdateActiveCamera();
        Shake(settings.Amplitude, settings.Duration);
    }

    /// <summary>
    /// Shake over a duration.
    /// </summary>
    public void Shake(float amplitude, float duration)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine(amplitude, duration));
    }

    public void Shake(float amplitude)
    {
        UpdateActiveCamera();
        perlinNoise.m_AmplitudeGain = amplitude;
    }

    public void StopShake()
    {
        perlinNoise.m_AmplitudeGain = 0f;
    }

    private IEnumerator ShakeRoutine(float amplitude, float duration)
    {
        perlinNoise.m_AmplitudeGain = amplitude;

        var elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        perlinNoise.m_AmplitudeGain = 0f;
        shakeCoroutine = null;
    }
}