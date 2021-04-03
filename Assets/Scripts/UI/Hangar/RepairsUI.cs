using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RepairsUI : MonoBehaviour
{
    [SerializeField] RepairTools repairTools;
    [SerializeField] private Text feedbackText;

    private void OnEnable()
    {
        repairTools.UpdateToolsText();
    }

    public void UpdateUI(bool wasSuccessful)
    {
        repairTools.UpdateToolsText();
        UpdateFeedbackText(wasSuccessful);
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