using UnityEngine;

/// <summary>Represents the possible states of an AudioSource</summary>
public enum AudioState
{
    Playing, Paused, Stopped
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    [field: SerializeField] public AudioClip[] AudioClips { get; private set; }

    protected AudioState currentState;
    public bool IsPlaying => currentState.Equals(AudioState.Playing);
    public bool IsPaused => currentState.Equals(AudioState.Paused);
    public bool IsStopped => currentState.Equals(AudioState.Stopped);

    protected void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            Debug.Log($"Playing audio clip '{audioClip.name}'");

            audioSource.clip = audioClip;
            audioSource.Play();
            currentState = AudioState.Playing;
        }
        else
        {
            Debug.LogError($"Audio clip {nameof(audioClip)} not found");
        }
    }

    protected void PauseAudioClip()
    {
        audioSource.Pause();
        currentState = AudioState.Paused;
    }

    protected void StopAudioClip()
    {
        audioSource.Stop();
        currentState = AudioState.Stopped;
    }
}
