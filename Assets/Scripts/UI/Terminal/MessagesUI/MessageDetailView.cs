using UnityEngine;
using UnityEngine.UI;

public class MessageDetailView : MonoBehaviour
{
    public Text messageSubjectText;
    public Text messageSenderText;
    public Text messageBodyText;
    public Button missionAcceptButton;
    public MissionDetailsUI missionDetailsUI;

    public void SetMessageDetails(Message message)
    {
        messageSubjectText.text = message.subject;
        messageSenderText.text = message.sender;

        if (string.IsNullOrEmpty(message.body))
        {
            messageBodyText.text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque tortor dui, elementum eu convallis non, cursus ac dolor. Quisque dictum est quam, et pellentesque velit rutrum eget. Nullam interdum ultricies velit pharetra aliquet. Integer sodales a magna quis ornare. Ut vulputate nibh ipsum. Vivamus tincidunt nec nisi in fermentum. Mauris consequat mi vel odio consequat, eget gravida urna lobortis. Pellentesque eu ipsum consectetur, pharetra nulla in, consectetur turpis. Curabitur ornare eu nisi tempus varius. Phasellus vel ex mauris. Fusce fermentum mi id elementum gravida.";

        }
        messageBodyText.text = messageBodyText.text.InsertNewLines();

        if (message.mission != null)
        {
            messageBodyText.text += "\n\nI've got a mission for you. See the details below:";
            messageBodyText.text += "\n\n" + missionDetailsUI.BuildDetailsString(message.mission);
        }
    }

    public void SetMissionAcceptButton(Mission mission)
    {
        Text buttonText = missionAcceptButton.GetComponentInChildren<Text>();
        buttonText.text = "Accept " + mission.missionName;

        missionAcceptButton.interactable = !mission.hasBeenAccepted;
        missionAcceptButton.AddOnClick(() =>
        {
            buttonText.text = "Mission Accepted!";
            missionAcceptButton.interactable = false;
            mission.hasBeenAccepted = true;
        });
    }
}