﻿using Events;
using Language.Lua;
using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;

public class DialogueDatabaseManager : MonoBehaviour
{
    public static DialogueDatabaseManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        RegisterEvents();
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

    public static string GetActorFieldAsString(string actorName, string fieldName)
    {
        if (DialogueUtils.ActorFieldExists(actorName, fieldName))
        {
            return DialogueLua.GetActorField(actorName, fieldName).asString;
        }

        Debug.LogError($"Lua actor '{actorName}' field '{fieldName}' does not exist");
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
            Debug.Log($"Lua variable '{variableName}' is now set to '{variable.asBool}'.");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            Debug.LogError($"Failed to set Lua variable '{variableName}'.");
        }
    }
}