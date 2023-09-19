using Language.Lua;
using PixelCrushers.DialogueSystem;
using System.Linq;
using UnityEngine;

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

    public static string GetSeenVariableName(this Conversation conversation)
    {
        if (!conversation.FieldExists(DialogueConstants.ConversationSeenVariableName)) 
        {
            return null;
        }
        return DialogueDatabaseManager.GetConversationFieldAsString(conversation, DialogueConstants.ConversationSeenVariableName);
    }

    public static bool IsConversationActive => DialogueManager.IsConversationActive;

    /// <summary>
    /// Because PixelCrushers didn't include this...
    /// </summary>
    /// <param name="conversationId"></param>
    /// <exception cref="System.Exception"></exception>
    public static void StartConversationById(int conversationId)
    {
        var conversation = GetConversationById(conversationId) ?? throw new System.Exception($"Conversation not found with ID '{conversationId}'");

        Debug.Log($"Starting conversation with Title '{conversation.Title}'...");
        DialogueManager.StartConversation(conversation.Title);
    }

    public static Conversation GetConversationById(int conversationId)
    {
        return DialogueManager.DatabaseManager.loadedDatabases.First().GetConversation(conversationId);
    }

    public static Conversation GetConversationByTitle(string title)
    {
        return DialogueManager.DatabaseManager.loadedDatabases.First().GetConversation(title);
    }

    public static Conversation GetLastStartedConversation()
    {
        return GetConversationByTitle(DialogueManager.lastConversationStarted);
    }

    public static DialogueEntry GetCurrentEntry()
    {
        var currentState = DialogueManager.Instance.CurrentConversationState;
        return currentState.subtitle.dialogueEntry;
    }
}
