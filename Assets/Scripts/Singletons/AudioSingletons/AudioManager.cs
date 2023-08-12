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

    protected void PlayAudioClip(AudioClip audioClip, AudioSource customSource = null)
    {
        if (audioClip == null)
        {
            Debug.LogError($"Audio clip {nameof(audioClip)} not found");
            return;
        }
        Debug.Log($"Playing audio clip '{audioClip.name}'");

        var source = customSource != null ? customSource : audioSource;
        source.clip = audioClip;
        source.Play();
        currentState = AudioState.Playing;
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
