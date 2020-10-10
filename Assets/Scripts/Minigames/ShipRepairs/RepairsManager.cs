using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

public class RepairsManager : MonoBehaviour
{
    // Tried to separate concerns using this 
    public int points;
    public int consecutiveWins;

    private string feedbackText;
    private string pointsText = "Points: ";
    private string consecutiveWinsText = "Consecutive Wins: ";
    private string successMessage = "Success!";
    private string failureMessage = "Failure!";

    public void UpdateText(bool success)
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

    private void OnGUI()
    {
        var localStyle = new GUIStyle();
        localStyle.normal.textColor = Color.black;

        GUI.Label(new Rect(
            128f, 64f, 128f, 128f), pointsText, localStyle);

        GUI.Label(new Rect(
            128f, 128f, 128f, 128f), consecutiveWinsText, localStyle);

        GUI.Label(new Rect(
            128f, 196f, 128f, 128f), feedbackText, localStyle);
    }
}