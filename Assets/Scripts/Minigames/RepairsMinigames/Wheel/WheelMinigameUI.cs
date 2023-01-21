using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WheelMinigameUI : MonoBehaviour
{
    [SerializeField]
    private Button wheelMinigameButton;

    [SerializeField]
    private Text feedbackText;

    private void Awake()
    {
        Wheel.OnRotationStarted += SetStopButtonState;
        Wheel.OnRotationStopped += SetStartButtonState;
    }

    private void SetStartButtonState()
    {
        wheelMinigameButton.SetText(RepairsConstants.StartButtonText);
    }

    private void SetStopButtonState()
    {
        wheelMinigameButton.SetText(RepairsConstants.StopButtonText);
        feedbackText.Clear();
    }

    public void AddStopStartListener(UnityAction callback)
    {
        wheelMinigameButton.AddOnClick(callback);
    }
}
