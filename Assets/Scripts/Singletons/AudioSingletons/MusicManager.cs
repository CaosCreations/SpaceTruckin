using System.Linq;
using UnityEngine;

public class MusicManager : AudioManager
{
    public static MusicManager Instance { get; private set; }
    private AudioClip CurrentTrack => Instance.AudioClips[currentTrackIndex];
    private int currentTrackIndex;

    [SerializeField] private bool trackIndexLoop;
    [SerializeField] private AudioClip titleScreenMusic;
    [SerializeField] private AudioClip creditsMusic;
    [SerializeField] private AudioClip[] musicWithAmbience;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                Debug.LogError($"Audio source in {nameof(MusicManager)} not found");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    protected override void PlayAudioClip(AudioClip audioClip, AudioSource customSource = null, bool fade = false, bool restartSameClip = true)
    {
        base.PlayAudioClip(audioClip, customSource, fade, restartSameClip);

        if (AmbientSoundEffectsManager.Instance == null)
        {
            return;
        }

        if (musicWithAmbience.Contains(audioClip))
        {
            AmbientSoundEffectsManager.Instance.PlaySoundEffect(AmbientSoundEffect.AsteroidAir);
        }
        else
        {
            AmbientSoundEffectsManager.Instance.PauseAudioClip();
        }
    }

    public void PlayTitleScreenMusic()
    {
        PlayAudioClip(titleScreenMusic, fade: true);
    }

    public void PlayCreditsMusic()
    {
        PlayAudioClip(creditsMusic, fade: true, restartSameClip: false);
    }

    private void PlayRandomTrack()
    {
        currentTrackIndex = GetRandomTrackIndex();
        PlayTrack();
    }

    private static int GetRandomTrackIndex()
    {
        return Random.Range(0, Instance.AudioClips.Length - 1);
    }

    public void PlayTrack()
    {
        PlayAudioClip(CurrentTrack);
    }

    public void PauseTrack()
    {
        PauseAudioClip();
    }

    public void StopTrack()
    {
        StopAudioClip();
    }

    public void ChangeTrack(bool isGoingForward)
    {
        if (isGoingForward)
        {
            currentTrackIndex = (currentTrackIndex + 1) % AudioClips.Length;
        }
        else if (currentTrackIndex >= 1)
        {
            currentTrackIndex--;
        }
        else
        {
            currentTrackIndex = Instance.AudioClips.Length - 1;
        }
    }

    public void SetUp()
    {
        if (trackIndexLoop)
        {
            // Override default looping 
            audioSource.loop = false;
        }

        currentTrackIndex = 0;
        PlayAudioClip(CurrentTrack, fade: true);
    }

    private void Update()
    {
        // AudioSource no longer playing so play next track
        if (hasPlayed && trackIndexLoop && !audioSource.isPlaying)
        {
            ChangeTrack(true);
            PlayAudioClip(CurrentTrack, fade: true);
        }
    }
}
