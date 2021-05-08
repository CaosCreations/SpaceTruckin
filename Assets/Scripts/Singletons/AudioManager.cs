using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioClip[] audioClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public static void PlayAudioClip(AudioSource audioSource, AudioClip audioClip) 
    {
        if (audioSource != null && audioClip != null)
        {
            Debug.Log($"Playing audio clip {audioClip.name}");
            
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.unityLogger.LogErrorIfObjectIsNull(audioSource);
            Debug.unityLogger.LogErrorIfObjectIsNull(audioClip);
        }
    }
}
