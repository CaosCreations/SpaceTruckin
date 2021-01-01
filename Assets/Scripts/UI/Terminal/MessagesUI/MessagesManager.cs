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
                GameObject newMessage = Instantiate(messageItemPrefab, scrollViewContent.transform);
                newMessage.name = "Message";
                MessageButtonHandler buttonHandler = newMessage.GetComponent<MessageButtonHandler>();
                buttonHandler.Init(message, () => GoToDetailView(message));
                //buttonHandler.button.AddOnClick(() => GoToDetailView(message));
                //GenerateMessageItem();
            }
        }
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
        
        //Text itemText = new GameObject().ScaffoldUI(
        //    name: "ItemText", parent: newItem, anchors: (Vector2.zero, Vector2.one)).AddComponent<Text>();

        //itemText.text = "NEW ITEM";

        //return newItem;
    }
}