using UnityEngine;
using UnityEngine.UI;

public class RepairsUI : MonoBehaviour
{
    [SerializeField] private Text toolsText;
    [SerializeField] private Text feedbackText;

    private void OnEnable()
    {
        UpdateToolsText();
    }

    public void UpdateUI(bool success)
    {
        UpdateToolsText();
        UpdateFeedbackText(success);
    }

    private void UpdateToolsText()
    {
        toolsText.SetText("x" + PlayerManager.Instance.RepairTools.ToString());
    }

    private void UpdateFeedbackText(bool success)
    {
        string feedback = success ? RepairsConstants.SuccessMessage : RepairsConstants.FailureMessage;
        Debug.Log(feedback);
        feedbackText.SetText(feedback);
    }

    public void ResetFeedbackText()
    {
        feedbackText.Clear();
    }
}