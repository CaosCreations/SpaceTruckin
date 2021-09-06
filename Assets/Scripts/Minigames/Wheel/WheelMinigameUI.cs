using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WheelMinigameUI : MonoBehaviour
{
    private Button stopStartButton;

    // Todo: Add feedback text to the reworked prefab
    [SerializeField] private Text feedbackText;

    private void Start()
    {
        stopStartButton = GetComponent<Button>();

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
