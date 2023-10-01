using System.Collections.Generic;

public class ConversationSeenInfo : IEditableDataModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public KeyValuePair<string, bool> SeenVariableKvp { get; set; }

    public string DisplayTitle => Title;

    public bool ToggleValue
    {
        get => SeenVariableKvp.Value;
        set
        {
            SeenVariableKvp = new KeyValuePair<string, bool>(SeenVariableKvp.Key, value);
        }
    }

    public override string ToString()
    {
        return $"{SeenVariableKvp.Key} == {SeenVariableKvp.Value}";
    }
}
