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
        messageSubjectText.text = message.Subject;
        messageSenderText.text = message.Sender;

        GameObject messageBody = Instantiate(messageBodyPrefab, messageBodyScrollViewContent.transform);
        RectTransform rectTransform = messageBody.GetComponent<RectTransform>();
        rectTransform.Reset();
        rectTransform.Stretch();

        Text messageBodyText = messageBody.GetComponent<Text>();
        string messageBodyContent;
        if (string.IsNullOrEmpty(message.body))
        {
            messageBodyContent = PlaceholderUtils.GenerateLoremIpsum(16);
        }
        else
        {
            messageBodyContent = message.body;
        }

        // Add mission information if a mission is offered in the message 
        if (message.Mission != null)
        {
            messageBodyContent += "\n\nI've got a mission for you. See the details below:";
            messageBodyContent += "\n\n" + missionDetailsUI.BuildDetailsString(message.Mission);
        }
        messageBodyText.SetText(messageBodyContent, FontManager.Instance.Aero);
    }

    public void SetMissionAcceptButton(Mission mission)
    {
        Text buttonText = missionAcceptButton.GetComponentInChildren<Text>();
        buttonText.text = "Accept " + mission.Name;

        missionAcceptButton.interactable = !mission.HasBeenAccepted;
        missionAcceptButton.AddOnClick(() =>
        {
            buttonText.text = "Mission Accepted!";
            missionAcceptButton.interactable = false;
            mission.HasBeenAccepted = true;
        });
    }
}