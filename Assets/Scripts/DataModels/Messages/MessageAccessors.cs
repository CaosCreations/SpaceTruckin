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

    public bool IsUnread 
    {
        get => saveData.isUnread; set => saveData.isUnread = value; 
    }

    public int Condition { get => condition; set => condition = value; }

    public Mission Mission { get => mission; set => mission = value; }
}
