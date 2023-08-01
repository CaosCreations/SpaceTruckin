using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShaker : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin;

    private float cachedNoiseAmplitudeGain;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        basicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cachedNoiseAmplitudeGain = basicMultiChannelPerlin.m_AmplitudeGain;

        basicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }

    public void ShakeScreen(float seconds)
    {
        StartCoroutine(ShakeScreenCoroutine(seconds));
    }

    private IEnumerator ShakeScreenCoroutine(float seconds) 
    {
        basicMultiChannelPerlin.m_AmplitudeGain = cachedNoiseAmplitudeGain;

        yield return new WaitForSeconds(seconds);

        basicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }
}
