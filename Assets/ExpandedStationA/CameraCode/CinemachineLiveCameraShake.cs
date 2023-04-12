using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineLiveCameraShake : CinemachineLiveCameraBehaviour
{
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    private Coroutine shakeCoroutine;

    protected override void Awake()
    {
        base.Awake();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(CameraShakeSettings settings)
    {
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