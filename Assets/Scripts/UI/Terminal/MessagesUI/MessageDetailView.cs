using UnityEngine;
using UnityEngine.UI;

public class MessageDetailView : MonoBehaviour
{
    public Text messageSubjectText;
    public Text messageSenderText;
    public GameObject messageBodyPrefab;
    public GameObject messageBodyScrollViewContent;
    
    public Button missionAcceptButton;
    public MissionDetailsUI missionDetailsUI;

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
        messageBodyContent = string.IsNullOrEmpty(message.body) ? PlaceholderUtils.GenerateLoremIpsum(16)
            : message.body;

        // Add mission information if a mission is offered in the message 
        if (message.Mission != null)
        {
            messageBodyContent += "\n\nI've got a mission for you. See the details below:";
            messageBodyContent += "\n\n" + missionDetailsUI.BuildDetailsString(message.Mission);
        }
        messageBodyText.SetText(messageBodyContent, FontType.Paragraph);
    }

    public void SetMissionAcceptButton(Mission mission)
    {
        missionAcceptButton.SetText("Accept " + mission.Name);

        missionAcceptButton.interactable = !mission.HasBeenAccepted;
        missionAcceptButton.AddOnClick(() =>
        {
            missionAcceptButton.SetText("Mission Accepted!");
            missionAcceptButton.interactable = false;
            mission.HasBeenAccepted = true;
        });
    }
}