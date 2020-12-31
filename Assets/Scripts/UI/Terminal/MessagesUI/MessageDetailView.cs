using UnityEngine;
using UnityEngine.UI;

public class MessageDetailView : MonoBehaviour
{
    public Text messageSubjectText;
    public Text messageSenderText;
    public Text messageBodyText;
    public Button missionAcceptButton;

    public void SetMessageDetails(Message message)
    {
        messageSubjectText.text = message.subject;
        messageSenderText.text = message.sender;
        messageBodyText.text = message.body.InsertNewLines();

        if (message.mission != null)
        {
            messageBodyText.text += "\nI've got a mission for you:";
            // Add mission details here (reuse code if possible)
            // 
        }
    }

    public void SetMissionAcceptButton(Mission mission)
    {
        Text buttonText = missionAcceptButton.GetComponentInChildren<Text>();
        buttonText.text = "Accept " + mission.missionName;

        missionAcceptButton.interactable = !mission.hasBeenUnlocked;
        missionAcceptButton.AddOnClick(() =>
        {
            buttonText.text = "Mission Accepted!";
            missionAcceptButton.interactable = false;
            //onJobAccept?.Invoke(mission);
        });

    }
}