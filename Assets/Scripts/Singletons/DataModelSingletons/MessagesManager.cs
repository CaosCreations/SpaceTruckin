using System;
using System.Linq;
using UnityEngine;

public class MessagesManager : MonoBehaviour, IDataModelManager
{
    public static MessagesManager Instance { get; private set; }

    [SerializeField] private MessageContainer messageContainer;
    public Message[] Messages 
    { 
        get => messageContainer.messages; set => messageContainer.messages = value; 
    }

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
        Init();
    }

    public void Init()
    {
        if (DataModelsUtils.SaveFolderExists(Message.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(Message.FOLDER_NAME);
        }
    }

    private void Start()
    {

    }

    public void UnlockMessages()
    {
        foreach (Message message in Instance.Messages)
        {
            if (message != null && !message.IsUnlocked)
            {
                if (PlayerManager.Instance.CanSpendMoney(message.Condition))
                {
                    message.IsUnlocked = true;
                }
            }
        }
    }

    public void AddNewMessage(Message message)
    {
        if (messageContainer.messages[messageContainer.messages.Length - 1] != null)
        {
            Array.Resize(ref messageContainer.messages, messageContainer.messages.Length + 1);
        }
        messageContainer.messages[messageContainer.messages.Length - 1] = message;
    }

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
}
