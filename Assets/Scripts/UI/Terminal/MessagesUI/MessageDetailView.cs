using UnityEngine;
using UnityEngine.UI;

public class MessageDetailView : MonoBehaviour
{
    [SerializeField] private Text messageSubjectText;
    [SerializeField] private Text messageSenderText;
    [SerializeField] private GameObject messageBodyPrefab;
    [SerializeField] private GameObject messageBodyScrollViewContent;

    public Button MissionAcceptButton;
    
    [SerializeField] private MissionDetailsUI missionDetailsUI;

    public void SetMessageDetails(Message message)
    {
        messageSubjectText.SetText(message.Subject, FontType.Title);
        messageSenderText.SetText(message.Sender, FontType.Subtitle);

        GameObject messageBody = Instantiate(messageBodyPrefab, messageBodyScrollViewContent.transform);

        RectTransform rectTransform = messageBody.GetComponent<RectTransform>();
        rectTransform.Reset();
        rectTransform.Stretch();

        Text messageBodyText = messageBody.GetComponent<Text>();
        string messageBodyContent;

        messageBodyContent = string.IsNullOrWhiteSpace(message.Body) 
            ? LoremIpsumGenerator.GenerateLoremIpsum(16)
            : message.Body;

        // Add mission information if a mission is offered in the message 
        if (message.Mission != null)
        {
            messageBodyContent += "\n\nI've got a mission for you. See the details below:";
            messageBodyContent += "\n\n" + missionDetailsUI.BuildDetailsString(message.Mission);
        }

        messageBodyText.SetText(messageBodyContent);
    }

    public void SetupMissionAcceptButton(Message message)
    {
        MissionAcceptButton.SetText("Accept " + message.Mission.Name);
        MissionAcceptButton.interactable = !message.Mission.HasBeenAccepted;

        MissionAcceptButton.AddOnClick(() => AcceptMissionViaMessage(message));
    }

    private void AcceptMissionViaMessage(Message message)
    {
        MissionAcceptButton.SetText(MessageConstants.MissionAcceptedText);
        MissionAcceptButton.interactable = false;

        message.Mission.AcceptMission();

        if (message.MissionBonus != null)
        {
            message.Mission.Bonus = message.MissionBonus;
        }
    }
}
