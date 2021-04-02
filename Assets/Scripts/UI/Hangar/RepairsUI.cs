using System.Text;
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

    public void UpdateUI(bool wasSuccessful)
    {
        UpdateToolsText();
        UpdateFeedbackText(wasSuccessful);
    }

    private void UpdateToolsText()
    {
        toolsText.SetText("x" + PlayerManager.Instance.RepairTools.ToString());
    }

    private void UpdateFeedbackText(bool wasSuccessful)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(
            wasSuccessful ? RepairsConstants.SuccessMessage : RepairsConstants.FailureMessage);
        
        builder.AppendLine($"You have {PlayerManager.Instance.RepairTools} tools remaining.");

        feedbackText.SetText(builder.ToString());
    }

    public void ResetFeedbackText()
    {
        feedbackText.Clear();
    }
}