using UnityEngine;
using UnityEngine.UI;

public class MessageDetailViewUI : MonoBehaviour
{
    [SerializeField] private Text messageSubjectText;
    [SerializeField] private Text messageSenderText;
    [SerializeField] private GameObject messageBodyPrefab;
    [SerializeField] private GameObject messageBodyScrollViewContent;

    public Button MissionAcceptButton;

    [SerializeField] private MissionDetailsUI missionDetailsUI;

    public void SetupDetailView(Message message)
    {
        ResetMessageDetails();
        SetMessageDetails(message);

        // Setup the mission accept button if the message offers a Mission 
        if (message.MissionProposition != null)
        {
            SetupMissionAcceptButton(message);
            MissionAcceptButton.SetActive(true);
        }
        else
        {
            MissionAcceptButton.SetActive(false);
        }

        // Apply the bonus if the message contains a MissionBonus and is unread. 
        // Not necessarily to the same Mission that is offered.
        if (!message.HasBeenRead && message.HasMissionBonus)
        {
            message.MissionToApplyBonusTo.Bonus = message.HasRandomBonus
                ? MissionsManager.GetRandomBonus()
                : message.MissionBonus;
        }
    }

    public void SetMessageDetails(Message message)
    {
        messageSubjectText.SetText(message.Subject, FontType.Title);
        messageSenderText.SetText(message.Sender, FontType.Subtitle);

        GameObject messageBody = Instantiate(messageBodyPrefab, messageBodyScrollViewContent.transform);

        RectTransform rectTransform = messageBody.GetComponent<RectTransform>();
        rectTransform.Reset();
        rectTransform.Stretch();

        Text messageBodyText = messageBody.GetComponent<Text>();

        string messageBodyContent = string.IsNullOrWhiteSpace(message.Body)
            ? LoremIpsumGenerator.GenerateLoremIpsum(MessageConstants.EmailLoremCount)
            : message.Body;

        // Add mission information if a Mission is offered in the message 
        if (message.MissionProposition != null)
        {
            messageBodyContent += "\n\nI've got a mission for you. See the details below:";
            messageBodyContent += "\n\n" + missionDetailsUI.BuildDetailsString(message.MissionProposition);
        }

        messageBodyText.SetText(messageBodyContent);
    }

    private void ResetMessageDetails()
    {
        messageBodyScrollViewContent.transform.DestroyDirectChildren();
    }

    public void SetupMissionAcceptButton(Message message)
    {
        MissionAcceptButton.SetText($"{MessageConstants.MissionAcceptButtonText} {message.MissionProposition.Name}");
        MissionAcceptButton.interactable = !message.MissionProposition.HasBeenAccepted;

        MissionAcceptButton.AddOnClick(() => AcceptMissionViaMessage(message.MissionProposition));
    }

    private void AcceptMissionViaMessage(Mission missionProposition)
    {
        MissionAcceptButton.SetText(MessageConstants.MissionAcceptedText);
        MissionAcceptButton.interactable = false;

        missionProposition.AcceptMission();
    }
}
