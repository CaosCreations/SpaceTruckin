using UnityEngine;

public class MessagesManager : MonoBehaviour, IDataModelManager
{
    public static MessagesManager Instance { get; private set; }

    [SerializeField] private MessageContainer messageContainer;
    public Message[] Messages { get => messageContainer.Messages; }

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
        if (DataUtils.SaveFolderExists(Message.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Message.FOLDER_NAME);
        }

        if (Messages == null)
        {
            Debug.LogError("No message data");
        }

        UnlockMessagesForTodaysDate();
        CalendarManager.OnEndOfDay += UnlockMessagesForTodaysDate;
    }

    /// <summary>
    /// Unlock messages that have a Total Money Earned condition that the player satisfies. 
    /// </summary>
    public void UnlockMessagesRequiringMoney()
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
    public void UnlockMessagesForTodaysDate() 
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
