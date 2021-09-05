using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WheelMinigameUI : MonoBehaviour
{
    [SerializeField] private Button stopStartButton;
    [SerializeField] private Text feedbackText;

    private void Start()
    {
        Wheel.OnRotationStarted += SetStopButtonState;
        Wheel.OnRotationStopped += SetStartButtonState;
    }

    private void SetStartButtonState()
    {
        stopStartButton.SetText(RepairsConstants.StartButtonText);
    }

    private void SetStopButtonState()
    {
        stopStartButton.SetText(RepairsConstants.StopButtonText);
        feedbackText.Clear();
    }

    public void AddStopStartListener(UnityAction callback)
    {
        stopStartButton.AddOnClick(callback);
    }
}
