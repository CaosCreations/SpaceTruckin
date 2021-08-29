using Language.Lua;
using PixelCrushers.DialogueSystem;

public static class DialogueUtils
{
    public static bool ActorExists(string actorName)
    {
        return DialogueLua.DoesTableElementExist("Actor", actorName);
    }

    public static LuaTable GetActorsTable()
    {
        return Lua.Environment.GetValue("Actor") is LuaTable actorsTable ? actorsTable : null;
    }

    public static bool VariableExists(string variableName)
    {
        return DialogueLua.DoesVariableExist(variableName);
    }

    public static bool ActorFieldExists(string actorName, string fieldName)
    {
        return DialogueLua.GetActorField(actorName, fieldName).luaValue != null;
    }

    public static bool IsConversationActive()
    {
        return DialogueManager.IsConversationActive;
    }
}
