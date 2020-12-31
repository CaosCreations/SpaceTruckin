using UnityEngine;
using UnityEngine.UI;

public class MessageDetailView : MonoBehaviour
{
    public Text messageDetail;
    public Button missionAcceptButton;

    public void SetMessage(Message message)
    {
        messageDetail.text = "From: " + message.sender + "\n";
        messageDetail.text += "Subject: " + message.subject + "\n\n";
        messageDetail.text += message.body + "\n";

        if (message.mission != null)
        {
            messageDetail.text += "I've got a mission for you.";
        }
    }

    public void SetupMissionAcceptButton(Mission mission)
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