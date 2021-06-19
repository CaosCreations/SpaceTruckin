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
}
