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
        messageBodyText.text = message.body.InsertNewLines();

        if (message.mission != null)
        {
            messageBodyText.text += "\nI've got a mission for you. See the details below:";
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
        });

    }
}