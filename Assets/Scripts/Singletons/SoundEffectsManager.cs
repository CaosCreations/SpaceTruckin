using UnityEngine;

public enum SoundEffect
{
	Button, None
}

public class SoundEffectsManager : MonoBehaviour
{
	public static SoundEffectsManager Instance { get; private set; }

	public AudioSource buttonSource;

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

	public static void PlaySoundEffect(SoundEffect soundEffect)
    {
		switch (soundEffect)
        {
			case SoundEffect.None:
				return;
			case SoundEffect.Button:
				Instance.buttonSource.Play();
				break;
        }
    }
}
