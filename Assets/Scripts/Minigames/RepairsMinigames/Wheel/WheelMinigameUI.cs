using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WheelMinigameUI : MonoBehaviour
{
    private Button wheelMinigameButton;

    private Text feedbackText;

    private void Awake()
    {
        Wheel.OnRotationStarted += SetStopButtonState;
        Wheel.OnRotationStopped += SetStartButtonState;

        wheelMinigameButton = RepairsMinigamesManager.GetRepairsMinigameButton(RepairsMinigameButtonType.A);
        feedbackText = RepairsMinigamesManager.GetRepairsMinigameFeedbackText();
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
