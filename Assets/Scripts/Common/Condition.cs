using System;
using System.Linq;

public enum ConditionType
{
    Date, Message, Mission, DialogueVariable,
}

public enum Operator
{
    And, Or,
}

[Serializable]
public class Condition
{
    public ConditionType Type;
    public DateWithPhase[] ActiveDates;
    public Message Message;
    public Mission Mission;
    public string DialogueVariableName;

    public bool IsMet => Type switch
    {
        ConditionType.Date => ActiveDates.Any(d => d.Date == CalendarManager.CurrentDate && d.Phase == ClockManager.CurrentTimeOfDayPhase),
        ConditionType.Message => Message.HasBeenRead,
        ConditionType.Mission => Mission.HasBeenCompleted,
        ConditionType.DialogueVariable => DialogueDatabaseManager.GetLuaVariableAsBool(DialogueVariableName),
        _ => false
    };
}
