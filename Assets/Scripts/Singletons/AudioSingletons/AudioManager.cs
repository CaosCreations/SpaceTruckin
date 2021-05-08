using UnityEngine;

/// <summary>Represents the possible states of an AudioSource</summary>
public enum AudioSourceState
{
    Playing, Paused, Stopped  
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [field: SerializeField] public AudioClip[] AudioClips { get; private set; }

    protected static AudioSourceState currentState;
    public static bool IsPlaying => currentState.Equals(AudioSourceState.Playing);
    public static bool IsPaused => currentState.Equals(AudioSourceState.Paused);
    public static bool IsStopped => currentState.Equals(AudioSourceState.Stopped);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource.LogIfNull();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public static void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            Debug.Log($"Playing audio clip {audioClip.name}");

            Instance.audioSource.clip = audioClip;
            Instance.audioSource.Play();
            currentState = AudioSourceState.Playing;
        }
        else
        {
            audioClip.LogIfNull();
        }
    }

    public static void PauseAudioClip()
    {
        Instance.audioSource.Pause();
        currentState = AudioSourceState.Paused;
        
    }

    public static void StopAudioClip()
    {
        Instance.audioSource.Stop();
        currentState = AudioSourceState.Stopped;
    }
}
