using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairsManager : MonoBehaviour
{
    public int points;
    public int consecutiveWins;

    private string feedbackText;
    private string pointsText = "Points: ";
    private string consecutiveWinsText = "Consecutive Wins: ";
    private string successMessage = "Success!";
    private string failureMessage = "Failure!";

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