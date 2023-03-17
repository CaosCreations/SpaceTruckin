using Cinemachine;
using UnityEngine;

public class CinemachineCameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    private float timer;

    [SerializeField]
    private bool start;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amplitude, float duration)
    {
        var perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlinNoise.m_AmplitudeGain = amplitude;
        timer = duration;
    }

    private void Update()
    {
        if (start)
        {
            Shake(5f, 5f);
            start = false;
        }

        if (timer <= 0)
            return;

        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            perlinNoise.m_AmplitudeGain = 0f;
            timer = 0f;
        }
    }
}
