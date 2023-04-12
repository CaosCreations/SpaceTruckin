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

    public void Shake(float amplitude, float duration)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeRoutine(amplitude, duration));
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