using Cinemachine;
using UnityEngine;

public class CinemachineCameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    private float timer;

    [Tooltip("Trigger the shake manually for testing")]
    [SerializeField]
    private bool start = false;

    [Tooltip("The strength of the shake")]
    [SerializeField]
    private float amplitude = 1f;

    [Tooltip("How long the shake lasts for")]
    [SerializeField]
    private float duration = .5f;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        Shake(amplitude, duration);
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
            Shake();
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
