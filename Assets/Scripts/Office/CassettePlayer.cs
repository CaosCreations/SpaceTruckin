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

	public Text currentTrackText;

	private AudioSource audioSource;
	private AudioClip firstTrack; 
	private AudioClip currentlyPlayingTrack;
	private CanvasManager canvasManager;

	private int currentTrackIndex;
	private bool trackPaused;

	private string currentTrackTextPrefix = "Current Track: ";

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = tracks[currentTrackIndex];

		canvasManager = GetComponent<CanvasManager>();
		trackPaused = true;

		AddListeners();
	}

	private void PlayTrack() 
	{
		SetAllButtonsInteractable();
		playButton.interactable = false;
		audioSource.clip = tracks[currentTrackIndex];
		audioSource.Play();
		// update UI 
		currentTrackText.text = tracks[currentTrackIndex].name; 
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
		// does it reset the firstTrack?
		// or just put the track back to the start
		audioSource.Stop();
		// audioSource.clip = firstTrack; 
	}

	private void ChangeTrack(bool next) 
	{
		// do we wrap around or stop at the first/last track?
		if (next && currentTrackIndex + 1 >= tracks.Length ||
			!next && currentTrackIndex - 1 < 0)
		{
			// maybe play a failed action sound effect here 
			// and/or grey out the button 
			return;
		}

		currentTrackIndex = next ? currentTrackIndex + 1 : currentTrackIndex - 1;

		audioSource.clip = tracks[currentTrackIndex];
		audioSource.Play();

		currentTrackText.text = "Current Track: " + tracks[currentTrackIndex].name;
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
