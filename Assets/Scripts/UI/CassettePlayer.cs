using UnityEngine;
using UnityEngine.UI;

public class CassettePlayer : MonoBehaviour
{
	public AudioClip[] tracks;

	public Button playButton;
	public Button pauseButton;
	public Button stopButton;
	public Button nextTrackButton;
	public Button previousTrackButton;

	private AudioSource audioSource;

	// Use these flags to differentiate between the 
	// player pressing pause/stop and the audiosource  
	// not playing 
	private bool trackStopped;
	private bool trackPaused;

	// Without this, when the user goes back/forward 
	// while the track is playing, we can't automatically
	// play the next track 
	private bool trackPlaying;

	private int currentTrackIndex;

	private void Start()
	{
		audioSource = FindObjectOfType<AudioSource>();
		audioSource.clip = tracks[currentTrackIndex];
		AddListeners();
	}

	private void AddListeners()
	{
		playButton.AddOnClick(() => PlayTrack());
		pauseButton.AddOnClick(() => PauseTrack());
		stopButton.AddOnClick(() => StopTrack());
		nextTrackButton.AddOnClick(() => ChangeTrack(goingForward: true));
		previousTrackButton.AddOnClick(() => ChangeTrack(goingForward: false));
	}

	private void PlayTrack() 
	{
		SetAllButtonsInteractable();
		playButton.interactable = false;
		ResetFlags();
		trackPlaying = true; 
		audioSource.clip = tracks[currentTrackIndex];
		audioSource.Play();
	}

	private void PauseTrack()
	{
		SetAllButtonsInteractable();
		pauseButton.interactable = false;
		ResetFlags(); 
		trackPaused = true;
		audioSource.Pause();
	}

	private void StopTrack()
	{
		SetAllButtonsInteractable();
		stopButton.interactable = false;
		ResetFlags();
		trackStopped = true;
		audioSource.Stop();
	}

	private void ChangeTrack(bool goingForward) 
	{
		if (goingForward)
        {
			if (currentTrackIndex < tracks.Length - 1)
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
				currentTrackIndex = tracks.Length - 1;
            }
        }
			
		audioSource.clip = tracks[currentTrackIndex];
		SetAllButtonsInteractable();

		// Don't grey out the following buttons if they are active,
		// since they will still be in effect 
		if (trackPlaying)
        {
			playButton.interactable = false;

			// Instantly play track when switching while previous track was active 
			audioSource.Play();
        }
		else
        {
			if (trackStopped)
            {
				stopButton.interactable = false;
            }
			else if (trackPaused)
            {
				pauseButton.interactable = false; 
            }
        }
	}

	// Call this before greying out the most recently pushed button 
	private void SetAllButtonsInteractable()
	{
		playButton.interactable = true;
		pauseButton.interactable = true;
		stopButton.interactable = true;
		nextTrackButton.interactable = true;
		previousTrackButton.interactable = true;
	}

	// Call this when playing, pausing, or stopping tracks 
	private void ResetFlags()
    {
		trackStopped = false;
		trackPaused = false;
		trackPlaying = false;
    }
}
