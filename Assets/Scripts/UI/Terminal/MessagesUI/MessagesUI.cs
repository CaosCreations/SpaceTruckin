using UnityEngine;
using UnityEngine.UI;

public enum MessageFilterMode 
{ 
    None, Unread, Read 
}

public class MessagesUI : MonoBehaviour
{
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private GameObject messageItemPrefab;
    [SerializeField] private GameObject messagesListView;
    [SerializeField] private GameObject messagesDetailView;
    [SerializeField] private MessageDetailViewUI messageDetailViewHandler;
    [SerializeField] private Button backButton;

    [SerializeField] private GameObject filterButtonContainer;
    [SerializeField] private Button unreadFilterButton;
    [SerializeField] private Button readFilterButton;

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
        MessagesManager.UnlockMessages();
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
        scrollViewContent.transform.DestroyDirectChildren();
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

                SetupButtonHandler(buttonHandler, message);
            }
        }
    }

    private void SetupButtonHandler(MessageButtonHandler buttonHandler, Message message)
    {
        buttonHandler.Init(message, () =>
        {
            GoToDetailView(message);

            // Set read flag to true upon opening the message  
            message.HasBeenRead = true;
        });
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
        if (currentFilterMode == MessageFilterMode.Unread && !message.HasBeenRead
            || currentFilterMode == MessageFilterMode.Read && message.HasBeenRead)
        {
            return true;
        }
        return false;
    }

    private Color GetMessageColour(Message message)
        => message.HasBeenRead ? MessageConstants.UnreadColour : MessageConstants.ReadColour;

    private void GoToDetailView(Message message)
    {
        // Hide the list view
        messagesListView.SetActive(false);
        filterButtonContainer.SetActive(false);
        
        // Show the detail view 
        messagesDetailView.SetActive(true);
        messageDetailViewHandler.SetupDetailView(message);
    }
}
