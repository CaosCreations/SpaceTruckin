using Events;
using UnityEngine;

public class MessagesManager : MonoBehaviour, IDataModelManager
{
    public static MessagesManager Instance { get; private set; }

    [SerializeField] private MessageContainer messageContainer;
    public Message[] Messages => messageContainer.Elements;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        if (Messages == null)
        {
            Debug.LogError("No message data");
            return;
        }

        UnlockMessages();
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
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

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        UnlockMessages();
    }

    private static bool IsMessageUnlockable(Message message)
    {
        if (message == null || message.IsUnlocked)
            return false;

        return message.UnlockCondition switch
        {
            MessageUnlockCondition.TotalMoney => message.IsUnlockedWithMoney && PlayerManager.Instance.CanSpendMoney(message.MoneyNeededToUnlock),
            MessageUnlockCondition.Date => CalendarManager.CurrentDate >= message.DateToUnlockOn,
            _ => false,
        };
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
