using Events;
using Language.Lua;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueDatabaseManager : MonoBehaviour
{
    public static DialogueDatabaseManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RegisterEvents();
        var subscribers = FindObjectsOfType<MonoBehaviour>().OfType<ILuaFunctionRegistrar>();
        foreach (var subscriber in subscribers)
        {
            subscriber.RegisterLuaFunctions();
        }
    }

    private void OnDisable()
    {
        var subscribers = FindObjectsOfType<MonoBehaviour>().OfType<ILuaFunctionRegistrar>();
        foreach (var subscriber in subscribers)
        {
            subscriber.UnregisterLuaFunctions();
        }
    }

    private void RegisterEvents()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
        SingletonManager.EventService.Add<OnBatteryChargedEvent>(OnBatteryChargedHandler);
    }

    /// <summary>
    /// Increase (or decrease if negative) an actor's Fondness field value, 
    /// i.e. its relationship points.
    /// </summary>
    /// <param name="actorName"></param>
    /// <param name="valueToAdd"></param>
    public static void AddToActorFondness(string actorName, int valueToAdd)
    {
        if (!string.IsNullOrWhiteSpace(actorName))
        {
            // Get the value of the fondness field from the actor in the dialogue database.
            Lua.Result actorFondnessField = DialogueLua.GetActorField(actorName, DialogueConstants.FondnessFieldName);

            if (actorFondnessField.luaValue == null)
            {
                Debug.LogError(
                    $"Actor '{actorName}' field '{DialogueConstants.FondnessFieldName}' does not exist");

                return;
            }

            int currentFondness = actorFondnessField.asInt;

            // Set the fondness field with the increased value.
            DialogueLua.SetActorField(
                actorName, DialogueConstants.FondnessFieldName, currentFondness + valueToAdd);
        }
    }

    /// <summary>
    /// Sets a field on an actor that tracks whether their conversation has been completed today. 
    /// </summary>
    /// <param name="hasCompleted"></param>
    public static void SetHasCompletedConversationToday(string actorName, bool hasCompleted)
    {
        Lua.Result hasCompletedField = DialogueLua.GetActorField(
            actorName, DialogueConstants.HasCompletedConversationTodayFieldName);

        if (hasCompletedField.luaValue == null)
        {
            Debug.LogError(
                $"Actor '{actorName}' field '{DialogueConstants.HasCompletedConversationTodayFieldName} does not exist");

            return;
        }

        DialogueLua.SetActorField(
            actorName, DialogueConstants.HasCompletedConversationTodayFieldName, hasCompleted);
    }

    /// <summary>
    /// Resets the field on each actor so that on the next day, the conversation completed condition will be met
    /// until a another conversation is completed that day. 
    /// </summary>
    private static void ResetHasCompletedConversationsToday()
    {
        // Get all actors in the dialogue database. 
        LuaTable actorsTable = DialogueUtils.GetActorsTable();

        foreach (var actorKey in actorsTable.Dict.Keys)
        {
            // Set each actor's conversation completed field to false. 
            DialogueLua.SetActorField(
                actorKey.ToString(), DialogueConstants.HasCompletedConversationTodayFieldName, false);
        }
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        ResetHasCompletedConversationsToday();
    }

    public static string GetLuaVariableAsString(string variableName)
    {
        if (DialogueUtils.VariableExists(variableName))
        {
            return DialogueLua.GetVariable(variableName).asString;
        }

        Debug.LogError($"Lua variable '{variableName}' does not exist");
        return string.Empty;
    }

    public static bool GetLuaVariableAsBool(string variableName)
    {
        if (DialogueUtils.VariableExists(variableName))
        {
            return DialogueLua.GetVariable(variableName).asBool;
        }

        Debug.LogError($"Lua variable '{variableName}' does not exist");
        return false;
    }

    public static string GetActorFieldAsString(string actorName, string fieldName)
    {
        if (DialogueUtils.ActorFieldExists(actorName, fieldName))
        {
            return DialogueLua.GetActorField(actorName, fieldName).asString;
        }

        Debug.LogError($"Lua actor '{actorName}' field '{fieldName}' does not exist");
        return string.Empty;
    }

    public static string GetConversationFieldAsString(Conversation conversation, string fieldName)
    {
        if (conversation.FieldExists(fieldName))
        {
            return DialogueLua.GetConversationField(conversation.id, fieldName).asString;
        }

        Debug.LogError($"Conversation field '{fieldName}' does not exist");
        return string.Empty;
    }

    public void OnBatteryChargedHandler(OnBatteryChargedEvent evt)
    {
        UpdateDatabaseVariable(evt.Args.Key, evt.Args.Value);
    }

    public void UpdateDatabaseVariable(string variableName, bool value)
    {
        Debug.Log($"Setting Lua variable '{variableName}' to value '{value}'...");
        try
        {
            DialogueLua.SetVariable(variableName, value);
            var variable = DialogueLua.GetVariable(variableName);
            Debug.Log($"Lua variable <color=green>{variableName}</color> is now set to <color=blue>{variable.asBool}</color>.");
            SingletonManager.EventService.Dispatch<OnDialogueVariableUpdatedEvent>();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            Debug.LogError($"Failed to set Lua variable '{variableName}'.");
        }
    }

    public static List<string> GetSeenVarNames()
    {
        var seenVarNames = new List<string>();

        foreach (var conversation in DialogueManager.DatabaseManager.DefaultDatabase.conversations)
        {
            var seenVarName = DialogueUtils.GetSeenVariableName(conversation);
            if (seenVarName != null)
            {
                seenVarNames.Add(seenVarName);
            }
        }
        return seenVarNames;
    }

    public static List<ConversationSeenInfo> GetSeenInfo()
    {
        var seenInfo = new List<ConversationSeenInfo>();

        foreach (var conversation in DialogueManager.DatabaseManager.DefaultDatabase.conversations)
        {
            var seenVarName = DialogueUtils.GetSeenVariableName(conversation);
            if (seenVarName != null)
            {
                var value = GetLuaVariableAsBool(seenVarName);
                seenInfo.Add(new ConversationSeenInfo
                {
                    Id = conversation.id,
                    Title = conversation.Title,
                    SeenVariableKvp = new KeyValuePair<string, bool>(seenVarName, value),
                });
            }
        }
        return seenInfo;
    }
}
