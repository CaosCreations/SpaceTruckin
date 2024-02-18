using UnityEngine;

public class MusicManager : AudioManager
{
    public static MusicManager Instance { get; private set; }
    private AudioClip CurrentTrack => Instance.AudioClips[currentTrackIndex];
    private int currentTrackIndex;

    [SerializeField] private bool trackIndexLoop;
    [SerializeField] private AudioClip titleScreenMusic;
    [SerializeField] private AudioClip mainStationMusic;

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

    public void PlayTitleScreenMusic()
    {
        PlayAudioClip(titleScreenMusic, fade: true);
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
