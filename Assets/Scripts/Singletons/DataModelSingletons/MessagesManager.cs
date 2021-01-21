using System.Linq;
using UnityEngine;

public class MessagesManager : MonoBehaviour, IDataModelManager
{
    public static MessagesManager Instance { get; private set; }

    [SerializeField] private MessageContainer messageContainer;
    public Message[] Messages { get => messageContainer.messages; }

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
        if (DataModelsUtils.SaveDataExists(Message.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(Message.FOLDER_NAME);
        }
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

    /// <summary>
    /// Creates a new message that will be sent to the player after they complete
    /// a mission for the first time.
    /// </summary>
    /// <param name="mission"></param>
    public void CreateThankyouMessage(Mission mission)
    {
        Message newMessage = ScriptableObject.CreateInstance<Message>();
        newMessage.Name = $"{mission.Name} followup.";

        // Placeholders
        newMessage.Subject = $"Thanks for completing {mission.Name}!";
        newMessage.Body = $"Hello Trucker. I really needed the {mission.Cargo}.";

        newMessage.IsUnlocked = true;
        Instance.Messages.ToList().Add(newMessage); 
        Instance.Messages.ToArray();
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
