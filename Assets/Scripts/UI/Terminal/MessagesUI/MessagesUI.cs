using UnityEngine;
using UnityEngine.UI;

public enum MessageFilterMode { None, Unread, Read }

public class MessagesUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject messageItemPrefab;
    public GameObject messagesListView;
    public GameObject messagesDetailView;
    public MessageDetailView messageDetailViewHandler;
    public Button backButton;

    public GameObject filterButtonContainer;
    public Button unreadFilterButton;
    public Button readFilterButton;

    // Determines what kinds of messages will appear in the list
    private MessageFilterMode currentFilterMode;

    private void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        backButton.AddOnClick(GoToListView);
        unreadFilterButton.AddOnClick(() => FilterMessages(MessageFilterMode.Unread));
        readFilterButton.AddOnClick(() => FilterMessages(MessageFilterMode.Read));
    }

    private void OnEnable()
    {
        MessagesManager.Instance.UnlockMessagesRequiringMoney();
        currentFilterMode = MessageFilterMode.None;
        GoToListView();
    }

    private void GoToListView()
    {
        messagesListView.SetActive(true);
        filterButtonContainer.SetActive(true);
        messagesDetailView.SetActive(false);
        CleanScrollView();
        AddMessages();
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
        foreach (Message message in MessagesManager.Instance.Messages)
        {
            if (MessageIsFilteredOut(message))
            {
                continue;
            }

            if (message != null && message.IsUnlocked)
            {
                GameObject newMessage = Instantiate(messageItemPrefab, scrollViewContent.transform);
                newMessage.name = "Message";
                newMessage.GetComponent<Image>().color = GetMessageColour(message);

                MessageButtonHandler buttonHandler = newMessage.GetComponent<MessageButtonHandler>();
                buttonHandler.Init(message, () =>
                {
                    GoToDetailView(message);
                    message.IsUnread = false; 
                });
            }
        }
    }

    private void FilterMessages(MessageFilterMode filterMode)
    {
        // Reset the filter mode if clicking that button for the second consecutive time
        if (filterMode == currentFilterMode)
        {
            currentFilterMode = MessageFilterMode.None;
        }
        else
        {
            currentFilterMode = filterMode;
        }
        CleanScrollView();
        AddMessages();
    }

    private bool MessageIsFilteredOut(Message message)
    {
        if (currentFilterMode == MessageFilterMode.Unread && !message.IsUnread
            || currentFilterMode == MessageFilterMode.Read && message.IsUnread)
        {
            return true;
        }
        return false;
    }

    private Color GetMessageColour(Message message) =>
        message.IsUnread ? MessageConstants.UnreadColour : MessageConstants.ReadColour;

    private void GoToDetailView(Message message)
    {
        messagesListView.SetActive(false);
        filterButtonContainer.SetActive(false);
        messagesDetailView.SetActive(true);
        messageDetailViewHandler.SetMessageDetails(message);

        if (message.Mission != null)
        {
            messageDetailViewHandler.SetMissionAcceptButton(message.Mission);
            messageDetailViewHandler.missionAcceptButton.gameObject.SetActive(true);
        }
        else
        {
            messageDetailViewHandler.missionAcceptButton.gameObject.SetActive(false);
        }
    }
}