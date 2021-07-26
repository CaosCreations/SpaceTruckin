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
    public MessageUnlockCondition UnlockCondition => unlockCondition;
    public bool IsUnlockedByDate => UnlockCondition == MessageUnlockCondition.Date;
    public bool IsUnlockedWithMoney => UnlockCondition == MessageUnlockCondition.TotalMoney;
    public Date DateToUnlockOn => dateToUnlockOn;
    public long MoneyNeededToUnlock { get => moneyNeededToUnlock; set => moneyNeededToUnlock = value; }
    public Mission Mission { get => missionProposition; set => missionProposition = value; }
    public MissionBonus MissionBonus { get => missionBonus; set => missionBonus = value; }
    public bool HasRandomBonus => hasRandomBonus;
}
