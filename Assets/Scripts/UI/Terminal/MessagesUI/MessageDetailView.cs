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
        Text messageBodyText = messageBody.GetComponent<Text>();

        if (string.IsNullOrEmpty(message.Body))
        {
            messageBodyText.text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque tortor dui, elementum eu convallis non, cursus ac dolor. Quisque dictum est quam, et pellentesque velit rutrum eget. Nullam interdum ultricies velit pharetra aliquet. Integer sodales a magna quis ornare. Ut vulputate nibh ipsum. Vivamus tincidunt nec nisi in fermentum. Mauris consequat mi vel odio consequat, eget gravida urna lobortis. Pellentesque eu ipsum consectetur, pharetra nulla in, consectetur turpis. Curabitur ornare eu nisi tempus varius. Phasellus vel ex mauris. Fusce fermentum mi id elementum gravida.";

        }
        messageBodyText.text = messageBodyText.text.InsertNewLines();

        if (message.Mission != null)
        {
            messageBodyText.text += "\n\nI've got a mission for you. See the details below:";
            messageBodyText.text += "\n\n" + missionDetailsUI.BuildDetailsString(message.Mission);
        }
    }

    public void SetMissionAcceptButton(Mission mission)
    {
        Text buttonText = missionAcceptButton.GetComponentInChildren<Text>();
        buttonText.text = "Accept " + mission.MissionName;

        missionAcceptButton.interactable = !mission.HasBeenAccepted;
        missionAcceptButton.AddOnClick(() =>
        {
            buttonText.text = "Mission Accepted!";
            missionAcceptButton.interactable = false;
            mission.HasBeenAccepted = true;
        });
    }
}