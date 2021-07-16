public partial class Message
{
    // This class is just for property accessors. 
    // The fields are all located in Message.cs. 
    
    /// <summary>
    /// The name of the message, not the object itself.
    /// </summary>
    public string Name
    {
        get => messageName; set => messageName = value;
    }
    public string Sender { get => sender; set => sender = value; }
    public string Subject { get => subject; set => subject = value; }
    public string Body { get => body; set => body = value; }
    public bool IsUnlocked
    {
        get => saveData.isUnlocked; set => saveData.isUnlocked = value;
    }
    public bool HasBeenRead 
    {
        get => saveData.hasBeenRead; set => saveData.hasBeenRead = value; 
    }
    public bool IsUnlockedByDate => unlockCondition == MessageUnlockCondition.Date;
    public bool IsUnlockedWithMoney => unlockCondition == MessageUnlockCondition.TotalMoney;
    public Date DateToUnlockOn => dateToUnlockOn;
    public long MoneyNeededToUnlock { get => moneyNeededToUnlock; set => moneyNeededToUnlock = value; }
    public Mission Mission { get => mission; set => mission = value; }
}
