using Language.Lua;
using PixelCrushers.DialogueSystem;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public struct FaceExpression
{
    public int Eye;
    public int Mouth;
    public int EyeIndex => Eye + 1;
    public int MouthIndex => Mouth + 1;
}

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

    public static string GetAccessVariableName(this Conversation conversation)
    {
        if (!conversation.FieldExists(DialogueConstants.AccessVariableName))
        {
            return null;
        }
        return DialogueDatabaseManager.GetConversationFieldAsString(conversation, DialogueConstants.AccessVariableName);
    }

    public static bool IsConversationActive => DialogueManager.Instance != null && DialogueManager.IsConversationActive;

    /// <summary>
    /// Because PixelCrushers didn't include this...
    /// </summary>
    /// <param name="conversationId"></param>
    /// <exception cref="System.Exception"></exception>
    public static void StartConversationById(int conversationId)
    {
        //var conversation = GetConversationById(conversationId) ?? throw new System.Exception($"Conversation not found with ID '{conversationId}'");
        var conversation = GetConversationById(conversationId);

        if (conversation == null)
        {
            return;
        }
        Debug.Log($"Starting conversation with Title '{conversation.Title}'...");
        DialogueManager.StartConversation(conversation.Title);
    }

    public static Conversation GetConversationById(int conversationId)
    {
        if (DialogueManager.DatabaseManager == null || DialogueManager.DatabaseManager.loadedDatabases.IsNullOrEmpty())
        {
            Debug.LogError("DatabaseManager was null or no databases were loaded. Can't get conversation by ID " + conversationId);
            return null;
        }
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

    private static readonly Regex ExpressionRegex = new Regex(@"^E(-?[01])M(-?[01])$", RegexOptions.Compiled);

    public static bool TryGetFaceExpression(DialogueEntry entry, out FaceExpression result)
    {
        result = default;

        if (entry?.fields == null)
        {
            return false;
        }

        var field = PixelCrushers.DialogueSystem.Field.Lookup(entry.fields, DialogueConstants.ExpressionFieldName);
        if (field == null || string.IsNullOrEmpty(field.value))
        {
            return false;
        }

        var match = ExpressionRegex.Match(field.value);
        if (!match.Success)
        {
            return false;
        }

        result.Eye = int.Parse(match.Groups[1].Value);
        result.Mouth = int.Parse(match.Groups[2].Value);
        return true;
    }

    public static int GetFacialExpressionIndex(DialogueEntry entry, int defaultValue = -1)
    {
        if (entry?.fields == null)
        {
            return defaultValue;
        }

        var field = PixelCrushers.DialogueSystem.Field.Lookup(entry.fields, DialogueConstants.FacialExpressionFieldName);
        if (field == null || string.IsNullOrEmpty(field.value))
        {
            return defaultValue;
        }

        return int.TryParse(field.value, out int index) ? index : defaultValue;
    }
}
