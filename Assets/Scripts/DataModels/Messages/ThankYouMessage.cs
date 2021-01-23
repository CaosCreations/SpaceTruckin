using System;
using System.Threading.Tasks;

public class ThankYouMessage : Message
{
    public new ThankYouMessageSaveData saveData; 
    
    [Serializable]
    public class ThankYouMessageSaveData : MessageSaveData
    {
        public string sender, subject, body;

        public ThankYouMessageSaveData(Mission mission)
        {
            sender = mission.Customer;
            subject = $"Thank for you completing {mission.Name}";
            body = $"I really needed that {mission.Cargo}!";
            isUnlocked = true;
            isUnread = false;
        }
    }

    public new bool IsUnread { get => saveData.isUnread; set => saveData.isUnread = value; }

    // Unlocked is always true, since we only create the message instance
    // when we need to send it, i.e. when the relevant mission is complete.
    public new bool IsUnlocked { get => true; }

    public async void Init(Mission mission)
    {
        ThankYouMessage newMessage = CreateInstance<ThankYouMessage>();
        newMessage.name = $"{mission.Name}_ThankYouMessage";
        if (!DataModelsUtils.SaveFileExists(newMessage.name))
        {
            newMessage.saveData = new ThankYouMessageSaveData(mission);
        }
        else
        {
            await LoadDataAsync();
        }
        MessagesManager.Instance.AddNewMessage(newMessage);
    }

    public override void SaveData()
    {
        if (!DataModelsUtils.SaveFileExists(name))
        {
            // Unlocked is always true, since we only create the message instance
            // when we need to send it, i.e. when the relevant mission is complete. 
            saveData.isUnlocked = true;

            saveData.isUnread = false;
        }
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async override Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<ThankYouMessageSaveData>(name, FOLDER_NAME);
    }
}
