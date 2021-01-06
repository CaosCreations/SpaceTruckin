using UnityEngine;
using UnityEngine.UI;

public class MessagesManager : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject messageItemPrefab;
    public MessageContainer messageContainer;

    public GameObject messagesListView;
    public GameObject messagesDetailView;
    public MessageDetailView messageDetailViewHandler;
    public Button backButton;

    private void Start()
    {
        backButton.AddOnClick(() => GoToListView());
        MissionsManager.Instance.onMissionCompleted += SendThankYouEmail;
    }

    private void OnEnable()
    {
        AddMessages();
        GoToListView();
    }

    public void CheckMessagesToAdd(long moneyMade)
    {
        UnlockMessages();
        CleanScrollView();
        AddMessages();
    }

    private void UnlockMessages()
    {
        foreach (Message message in messageContainer.messages)
        {
            if (!message.unlocked)
            {
                if (PlayerManager.Instance.playerData.playerMoney > message.condition)
                {
                    message.unlocked = true;
                }
            }
        }
    }

    private void CleanScrollView()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void AddMessages()
    {
        foreach (Message message in messageContainer.messages)
        {
            if (message != null && message.unlocked)
            {
                AddMessage(message);
            }
        }
    }

    private void AddMessage(Message message)
    {
        GameObject newMessage = Instantiate(messageItemPrefab, scrollViewContent.transform);
        newMessage.name = "Message";
        MessageButtonHandler buttonHandler = newMessage.GetComponent<MessageButtonHandler>();
        buttonHandler.Init(message, () => GoToDetailView(message));
    }

    private void GoToDetailView(Message message)
    {
        messagesListView.SetActive(false);
        messagesDetailView.SetActive(true);
        messageDetailViewHandler.SetMessageDetails(message);

        if (message.mission != null)
        {
            messageDetailViewHandler.SetMissionAcceptButton(message.mission);
            messageDetailViewHandler.missionAcceptButton.gameObject.SetActive(true);
        }
        else
        {
            messageDetailViewHandler.missionAcceptButton.gameObject.SetActive(false);
        }
    }

    private void GoToListView()
    {
        messagesListView.SetActive(true);
        messagesDetailView.SetActive(false);
    }

    public void GenerateMessageItem()
    {
        GameObject newItem = new GameObject().ScaffoldUI(
            name: "MessageItem", parent: scrollViewContent, anchors: (Vector2.zero, Vector2.one));

        newItem.AddComponent<Button>();
        newItem.AddComponent<MessageButtonHandler>();
    }

    private void SendThankYouEmail(Mission mission)
    {
        Debug.Log("Sending thank you email");

        Message newMessage = ScriptableObject.CreateInstance<Message>();
        newMessage.sender = mission.customer;
        newMessage.subject = "Thank you!";

        if (string.IsNullOrEmpty(mission.thankYouMessage))      
        {
            newMessage.body = $"Hey pal,\nThanks for helping me out with {mission.missionName}";
        }
        else
        {
            newMessage.body = mission.thankYouMessage;
        }
        newMessage.unlocked = true;
        messageContainer.messages[messageContainer.messages.Length] = newMessage;
        AddMessage(newMessage);
    }
}