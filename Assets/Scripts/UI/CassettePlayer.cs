using System.Collections;
using System.Collections.Generic;
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
		playButton.onClick.AddListener(() => PlayTrack());
		pauseButton.onClick.AddListener(() => PauseTrack());
		stopButton.onClick.AddListener(() => StopTrack());
		nextTrackButton.onClick.AddListener(() => ChangeTrack(next: true));
		previousTrackButton.onClick.AddListener(() => ChangeTrack(next: false));
	}

	private void PlayTrack() 
	{
		SetAllButtonsInteractable();
		playButton.interactable = false;
		ResetFlags();
		trackPlaying = true; 
		audioSource.clip = tracks[currentTrackIndex];
		audioSource.Play();

		Debug.Log("Current track playing: " + audioSource.clip.name); 
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

	private void ChangeTrack(bool next) 
	{
		// We could wrap around to the first/last track, 
		// but this may be less authentic (it's a cassette player) 
		if (next && currentTrackIndex + 1 >= tracks.Length ||
			!next && currentTrackIndex - 1 < 0)
		{
			return;
		}

		currentTrackIndex = next ? currentTrackIndex + 1 : currentTrackIndex - 1;
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
