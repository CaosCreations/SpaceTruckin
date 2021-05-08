using UnityEngine;

public class MusicManager : AudioManager
{
    public static MusicManager Instance { get; private set; }

    private int currentTrackIndex;
    private AudioClip CurrentTrack => Instance.AudioClips[currentTrackIndex];

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

    private void Start()
    {
        PlayMusicOnStart();
    }

    private void PlayMusicOnStart()
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
        else
        {
            if (currentTrackIndex >= 1)
            {
                currentTrackIndex--;
            }
            else
            {
                currentTrackIndex = Instance.AudioClips.Length - 1;
            }
        }
    }
}
