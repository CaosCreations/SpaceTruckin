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
	private AudioClip firstTrack; 
	private AudioClip currentlyPlayingTrack;

	private CanvasManager canvasManager;

	private int currentTrackIndex;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = tracks[currentTrackIndex];
		
		canvasManager = GetComponent<CanvasManager>();

		AddListeners();
	}

	private void PlayTrack() 
	{
		SetAllButtonsInteractable();
		playButton.interactable = false;
		audioSource.clip = tracks[currentTrackIndex];
		audioSource.Play();
	}

	private void PauseTrack()
	{
		SetAllButtonsInteractable();
		pauseButton.interactable = false;
		audioSource.Pause();
		// update UI 
	}

	private void StopTrack()
	{
		SetAllButtonsInteractable();
		stopButton.interactable = false;
		audioSource.Stop();
	}

	private void ChangeTrack(bool next) 
	{
		// We could wrap around to the first/last track, 
		// but this may be less authentic and unnecessary
		if (next && currentTrackIndex + 1 >= tracks.Length ||
			!next && currentTrackIndex - 1 < 0)
		{
			return;
		}

		currentTrackIndex = next ? currentTrackIndex + 1 : currentTrackIndex - 1;

		// Grey out the corresponding button 
		if (next)
        {
			nextTrackButton.interactable = false;
        }
		else
        {
			previousTrackButton.interactable = false; 
        }

		audioSource.clip = tracks[currentTrackIndex];
		audioSource.Play();
	}

	private void AddListeners()
	{
		playButton.onClick.AddListener(() => PlayTrack());
		pauseButton.onClick.AddListener(() => PauseTrack());
		stopButton.onClick.AddListener(() => StopTrack());
		nextTrackButton.onClick.AddListener(() => ChangeTrack(next: true));
		previousTrackButton.onClick.AddListener(() => ChangeTrack(next: false));
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
	
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (Input.GetKeyDown(PlayerConstants.action))
			{
				canvasManager.ActivateCanvas();
			}
			else if (Input.GetKeyDown(PlayerConstants.exit))
			{
				canvasManager.DeactivateCanvas();
			}
		}
	}
}
