using System;
using System.Threading.Tasks;

public class ThankYouMessage : Message
{
    public new ThankYouMessageSaveData saveData; 
    
    [Serializable]
    public class ThankYouMessageSaveData : MessageSaveData
    {
        public string sender, subject, body;
    }

    public async void Init(Mission mission)
    {
        ThankYouMessage newMessage = CreateInstance<ThankYouMessage>();
        newMessage.name = $"{mission.Name}_ThankYouMessage";
        await LoadDataAsync();
        
        MessagesManager.Instance.AddNewMessage(newMessage);
    }

    public override void SaveData()
    {
        if (!DataModelsUtils.SaveFileExists(name))
        {
            // Unlocked is always true, since we only create the message instance
            // when we need to send it, i.e. when the relevant mission is complete. 
            saveData.isUnlocked = true;
        }
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async override Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<ThankYouMessageSaveData>(name, FOLDER_NAME);
    }
}
