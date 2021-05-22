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
		SetButtonInteractability(AudioState.Playing);
		MusicManager.Instance.PlayTrack();
	}

	private void PauseTrack()
	{
		SetButtonInteractability(AudioState.Paused);
		MusicManager.Instance.PauseTrack();
	}

	private void StopTrack()
	{
		SetButtonInteractability(AudioState.Stopped);
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

	private void SetButtonInteractability(AudioState currentState)
    {
        SetAllButtonsInteractable();

        switch (currentState)
        {
			case AudioState.Playing:
				playButton.interactable = false;
				break;
			case AudioState.Paused:
				pauseButton.interactable = false;
				break;
			case AudioState.Stopped:
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
