using System.Collections.Generic;

public class ConversationSeenInfo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public KeyValuePair<string, bool> SeenVariableKvp { get; set; }

    public override string ToString()
    {
        return $"{SeenVariableKvp.Key} == {SeenVariableKvp.Value}";
    }
}
