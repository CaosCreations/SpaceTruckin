using UnityEngine;

public class MusicManager : AudioManager
{
    private static int currentTrackIndex;
    private static AudioClip CurrentTrack => Instance.AudioClips[currentTrackIndex];

    private void Start()
    {
        PlayMusicOnStart();
    }

    private static void PlayMusicOnStart()
    {
        currentTrackIndex = GetRandomTrackIndex();
        PlayTrack();
    }

    private static int GetRandomTrackIndex()
    {
        return Random.Range(0, Instance.AudioClips.Length - 1);
    }

    public static void PlayTrack()
    {
        PlayAudioClip(CurrentTrack);
    }

    public static void PauseTrack()
    {
        PauseAudioClip();
    }

    public static void StopTrack()
    {
        StopAudioClip();
    }

    public static void ChangeTrack(bool isGoingForward)
    {
        if (isGoingForward)
        {
            if (currentTrackIndex < Instance.AudioClips.Length - 1)
            {
                currentTrackIndex++;
            }
            else
            {
                currentTrackIndex = 0;
            }
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
