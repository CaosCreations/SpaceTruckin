using UnityEngine;

public class MessagesManager : MonoBehaviour, IDataModelManager
{
    public static MessagesManager Instance { get; private set; }

    [SerializeField] private MessageContainer messageContainer;
    public Message[] Messages { get => messageContainer.Elements; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Init()
    {
        if (Messages == null)
        {
            Debug.LogError("No message data");
        }

        UnlockMessages();
        CalendarManager.OnEndOfDay += UnlockMessages;
    }

    public static void UnlockMessages()
    {
        foreach (Message message in Instance.Messages)
        {
            if (IsMessageUnlockable(message))
            {
                message.IsUnlocked = true;
            }
        }
    }

    /// <summary>
    /// Unlock messages that have a Total Money Earned condition that the player satisfies. 
    /// </summary>
    public static void UnlockMessagesRequiringMoney()
    {
        foreach (Message message in Instance.Messages)
        {
            if (message != null
                && !message.IsUnlocked
                && message.IsUnlockedWithMoney
                && PlayerManager.Instance.CanSpendMoney(message.MoneyNeededToUnlock))
            {
                message.IsUnlocked = true;
            }
        }
    }

    /// <summary>
    /// Unlock messages that have a Date condition that the current Date surpasses.
    /// </summary>
    public static void UnlockMessagesForTodaysDate()
    {
        foreach (Message message in Instance.Messages)
        {
            if (message != null
                && !message.IsUnlocked
                && message.IsUnlockedByDate
                && CalendarManager.Instance.CurrentDate >= message.DateToUnlockOn)
            {
                message.IsUnlocked = true;
            }
        }
    }

    private static bool IsMessageUnlockable(Message message)
    {
        if (message == null || message.IsUnlocked)
            return false;

        switch (message.UnlockCondition)
        {
            case MessageUnlockCondition.TotalMoney:
                return message.IsUnlockedWithMoney
                    && PlayerManager.Instance.CanSpendMoney(message.MoneyNeededToUnlock);

            case MessageUnlockCondition.Date:
                return CalendarManager.Instance.CurrentDate >= message.DateToUnlockOn;

            default:
                return false;
        }
    }

    #region Persistence
    public void SaveData()
    {
        foreach (Message message in Instance.Messages)
        {
            if (message != null)
            {
                message.SaveData();
            }
        }
    }

    public async void LoadDataAsync()
    {
        if (!DataUtils.SaveFolderExists(Message.FolderName))
        {
            DataUtils.CreateSaveFolder(Message.FolderName);
            return;
        }

        foreach (Message message in Instance.Messages)
        {
            if (message != null)
            {
                await message.LoadDataAsync();
            }
        }
    }

    public void DeleteData()
    {
        Instance.DeleteData();
    }
    #endregion
}
