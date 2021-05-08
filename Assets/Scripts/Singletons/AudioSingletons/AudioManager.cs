using UnityEngine;

/// <summary>Represents the possible states of an AudioSource</summary>
public enum AudioSourceState
{
    Playing, Paused, Stopped  
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    [field: SerializeField] public AudioClip[] AudioClips { get; private set; }

    protected AudioSourceState currentState;
    public bool IsPlaying => currentState.Equals(AudioSourceState.Playing);
    public bool IsPaused => currentState.Equals(AudioSourceState.Paused);
    public bool IsStopped => currentState.Equals(AudioSourceState.Stopped);

    protected void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            Debug.Log($"Playing audio clip '{audioClip.name}'");

            audioSource.clip = audioClip;
            audioSource.Play();
            currentState = AudioSourceState.Playing;
        }
        else
        {
            audioClip.LogIfNull();
        }
    }

    protected void PauseAudioClip()
    {
        audioSource.Pause();
        currentState = AudioSourceState.Paused;
    }

    protected void StopAudioClip()
    {
        audioSource.Stop();
        currentState = AudioSourceState.Stopped;
    }
}
