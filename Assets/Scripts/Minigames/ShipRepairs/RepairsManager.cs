using UnityEngine;

public class RepairsManager : MonoBehaviour
{
    private Workstation workstation;
    private GreenZone greenZone;

    public int points;
    public int consecutiveWins;

    private string feedbackText;
    private string pointsText = "Points: ";
    private string consecutiveWinsText = "Consecutive Wins: ";
    private string successMessage = "Success!";
    private string failureMessage = "Failure!";

    private void Start()
    {
        workstation = GetComponentInChildren<Workstation>();
        greenZone = GetComponentInChildren<GreenZone>();
    }

    public void PlayerWins()
    {
        points++;
        consecutiveWins++;
        workstation.IncreaseRotationSpeed();

        // Decrease green zone size every n wins 
        if (IsGreenZoneShrinking())
        {
            greenZone.ReduceSize();
        }

        if (IsDirectionReversing())
        {
            workstation.ReverseRotationDirection();
        }
        UpdateFeedbackText(true);
    }

    public void PlayerLoses()
    {
        consecutiveWins = 0;
        workstation.ResetRotationSpeed();
        greenZone.ResetSize();
        UpdateFeedbackText(success: false);
    }
    //
    // 
    public bool IsGreenZoneShrinking()
    {
        return consecutiveWins % RepairsConstants.greenZoneShrinkInterval == 0
            && consecutiveWins > 0;
    }

    public bool IsDirectionReversing()
    {
        return Random.Range(0, RepairsConstants.rotationReversalUpperBound)
            > RepairsConstants.rotationReversalThreshold;
    }

    public void UpdateFeedbackText(bool success)
    {
        feedbackText = success ? successMessage : failureMessage;
        Debug.Log(feedbackText);
        pointsText = "Points: " + points;
        consecutiveWinsText = "Consecutive Wins: " + consecutiveWins;
    }

    public void ResetFeedbackText()
    {
        feedbackText = string.Empty;
    }
}