using UnityEngine;
using UnityEngine.UI;

public class CassettePlayer : UICanvasBase
{
	[SerializeField] private Button playButton;
	[SerializeField] private Button pauseButton;
	[SerializeField] private Button stopButton;
	[SerializeField] private Button nextTrackButton;
	[SerializeField] private Button previousTrackButton;

	private void Start()
	{
		AddListeners();
	}

	private void AddListeners()
	{
		playButton.AddOnClick(() => PlayTrack());
		pauseButton.AddOnClick(() => PauseTrack());
		stopButton.AddOnClick(() => StopTrack());
		nextTrackButton.AddOnClick(() => ChangeTrack(isGoingForward: true));
		previousTrackButton.AddOnClick(() => ChangeTrack(isGoingForward: false));
	}

	private void PlayTrack()
    {
		SetButtonInteractability(AudioSourceState.Playing);
		MusicManager.Instance.PlayTrack();
	}

	private void PauseTrack()
	{
		SetButtonInteractability(AudioSourceState.Paused);
		MusicManager.Instance.PauseTrack();
	}

	private void StopTrack()
	{
		SetButtonInteractability(AudioSourceState.Stopped);
		MusicManager.Instance.StopTrack();
	}

	private void ChangeTrack(bool isGoingForward)
    {
		MusicManager.Instance.ChangeTrack(isGoingForward);

		if (MusicManager.Instance.IsPlaying)
        {
			MusicManager.Instance.PlayTrack();
        }
    }

	private void SetButtonInteractability(AudioSourceState currentState)
    {
        SetAllButtonsInteractable();

        switch (currentState)
        {
			case AudioSourceState.Playing:
				playButton.interactable = false;
				break;
			case AudioSourceState.Paused:
				pauseButton.interactable = false;
				break;
			case AudioSourceState.Stopped:
				stopButton.interactable = false;
				break;
		}
	}

	private void SetAllButtonsInteractable()
	{
		playButton.interactable = true;
		pauseButton.interactable = true;
		stopButton.interactable = true;
		nextTrackButton.interactable = true;
		previousTrackButton.interactable = true;
	}
}
