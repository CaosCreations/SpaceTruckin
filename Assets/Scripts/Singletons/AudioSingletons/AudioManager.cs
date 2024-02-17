using System.Collections;
using UnityEngine;

/// <summary>Represents the possible states of an AudioSource</summary>
public enum AudioState
{
    Playing, Paused, Stopped
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] private float crossfadeDuration = 2f;
    [field: SerializeField] public AudioClip[] AudioClips { get; private set; }

    protected AudioState currentState;
    public bool IsPlaying => currentState.Equals(AudioState.Playing);
    public bool IsPaused => currentState.Equals(AudioState.Paused);
    public bool IsStopped => currentState.Equals(AudioState.Stopped);

    protected void PlayAudioClip(AudioClip audioClip, AudioSource customSource = null, bool fade = false)
    {
        if (audioClip == null)
        {
            Debug.LogError($"Audio clip {nameof(audioClip)} not found");
            return;
        }
        //Debug.Log($"Playing audio clip '{audioClip.name}'");

        var source = customSource != null ? customSource : audioSource;

        if (fade)
        {
            CrossfadeAudioClips(audioClip, source);
        }
        else
        {
            PlayAudioClip(audioClip, source);
        }
        currentState = AudioState.Playing;
    }

    private void PlayAudioClip(AudioClip audioClip, AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.pitch = 1;
        audioSource.Play();
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

    private void CrossfadeAudioClips(AudioClip audioClip, AudioSource audioSource)
    {
        StartCoroutine(Crossfade(audioClip, audioSource));
    }

    private IEnumerator Crossfade(AudioClip audioClip, AudioSource audioSource)
    {
        var elapsedTime = 0f;
        var startVolume = audioSource.volume;

        if (audioSource.isPlaying)
        {
            // Fade out
            while (elapsedTime < crossfadeDuration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / crossfadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            audioSource.Stop();
        }

        // Fade in 
        PlayAudioClip(audioClip, audioSource);

        elapsedTime = 0f;
        while (elapsedTime < crossfadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, elapsedTime / crossfadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
