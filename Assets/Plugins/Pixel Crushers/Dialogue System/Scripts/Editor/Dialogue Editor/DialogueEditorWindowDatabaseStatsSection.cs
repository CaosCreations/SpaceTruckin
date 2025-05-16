// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace PixelCrushers.DialogueSystem.DialogueEditor
{

    /// <summary>
    /// This part of the Dialogue Editor window handles the Database tab's Stats section.
    /// </summary>
    public partial class DialogueEditorWindow
    {

        public class DatabaseStats
        {
            public bool isValid = false;

            public int numActors;
            public int numQuests;
            public int numVariables;
            public int numConversations;
            public int numDialogueEntries;
            public int numDialogueEntriesNonBlank;
            public int numSceneEvents;

            public int questWordCount;
            public int conversationWordCount;
            public int totalWordCount;

            public bool actorStatsFoldout = false;
            public Dictionary<string, ActorStats> actorStats = new Dictionary<string, ActorStats>();
        }

        public class ActorStats
        {
            public HashSet<int> conversationIDs = new HashSet<int>();
            public int numDialogueEntries = 0;
            public int numWords = 0;
        }

        private DatabaseStats stats = new DatabaseStats();
        private Dictionary<int, string> actorStatsKeys = new Dictionary<int, string>();

        private void DrawStatsSection()
        {
            EditorWindowTools.StartIndentedSection();

            EditorGUI.BeginDisabledGroup(database == null);
            if (GUILayout.Button("Update Stats"))
            {
                UpdateStats();
            }
            EditorGUI.EndDisabledGroup();
            if (stats.isValid)
            {
                EditorGUILayout.LabelField("Asset Count", EditorStyles.boldLabel);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.IntField("Actors", stats.numActors);
                EditorGUILayout.IntField("Quests", stats.numQuests);
                EditorGUILayout.IntField("Variables", stats.numVariables);
                EditorGUILayout.IntField("Conversations", stats.numConversations);
                EditorGUILayout.IntField("Dialogue Entries", stats.numDialogueEntries);
                EditorGUILayout.IntField("Entries non-blank", stats.numDialogueEntriesNonBlank);
                EditorGUILayout.IntField("Scene Events", stats.numSceneEvents);
                EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.LabelField("Word Count", EditorStyles.boldLabel);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.IntField("Quests", stats.questWordCount);
                EditorGUILayout.IntField("Conversations", stats.conversationWordCount);
                EditorGUILayout.IntField("Total", stats.totalWordCount);
                EditorGUI.EndDisabledGroup();
                stats.actorStatsFoldout = EditorGUILayout.Foldout(stats.actorStatsFoldout, "Actor Stats");
                if (stats.actorStatsFoldout) DrawActorStats();
            }
            EditorWindowTools.EndIndentedSection();
        }

        private void DrawActorStats()
        {
            EditorGUI.BeginDisabledGroup(true);
            foreach (var kvp in stats.actorStats)
            {
                EditorGUILayout.LabelField(kvp.Key);
                EditorGUI.indentLevel++;
                EditorGUILayout.IntField("Conversations", kvp.Value.conversationIDs.Count);
                EditorGUILayout.IntField("Dialogue Entries", kvp.Value.numDialogueEntries);
                EditorGUILayout.IntField("Words", kvp.Value.numWords);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndDisabledGroup();
        }

        private void UpdateStats()
        {
            if (database == null) return;
            stats.isValid = true;

            try
            {
                stats.numDialogueEntries = stats.numDialogueEntriesNonBlank = stats.numSceneEvents = 0;
                stats.questWordCount = stats.conversationWordCount = 0;
                stats.actorStats.Clear();
                actorStatsKeys.Clear();

                EditorUtility.DisplayProgressBar("Computing Stats", "Actors", 0);
                stats.numActors = database.actors.Count;

                EditorUtility.DisplayProgressBar("Computing Stats", "Quests", 1);
                foreach (var quest in database.items)
                {
                    if (quest.IsItem) continue;
                    stats.numQuests++;
                    foreach (var field in quest.fields)
                    {
                        if (!(field.type == FieldType.Text || field.type == FieldType.Localization)) continue;
                        stats.questWordCount += GetWordCount(field.value);
                    }
                }

                EditorUtility.DisplayProgressBar("Computing Stats", "Variables", 2);
                stats.numVariables = database.variables.Count;

                stats.numConversations = database.conversations.Count;
                for (int i = 0; i < stats.numConversations; i++)
                {
                    var progress = (float)i / (float)stats.numConversations;
                    EditorUtility.DisplayProgressBar("Computing Stats", "Conversations", progress);
                    var conversation = database.conversations[i];
                    foreach (var entry in conversation.dialogueEntries)
                    {
                        // Get actor/conversant info and add conversation ID:
                        var actorID = entry.ActorID;
                        if (!actorStatsKeys.TryGetValue(actorID, out var actorKey))
                        {
                            var actor = database.GetActor(actorID);
                            actorKey = (actor != null) ? $"[{actorID}] {actor.Name}" : $"[{actorID}] (Unknown)";
                            actorStatsKeys[actorID] = actorKey;
                        }
                        if (!stats.actorStats.TryGetValue(actorKey, out var actorStats))
                        {
                            actorStats = new ActorStats();
                            stats.actorStats[actorKey] = actorStats;
                        }
                        actorStats.conversationIDs.Add(conversation.id);
                        if (entry.id != 0) actorStats.numDialogueEntries++;
                        var conversantID = entry.ConversantID;
                        if (!actorStatsKeys.TryGetValue(conversantID, out var conversantKey))
                        {
                            var conversant = database.GetActor(conversantID);
                            conversantKey = (conversant != null) ? $"[{conversantID}] {conversant.Name}" : $"[{conversantID}] (Unknown)";
                            actorStatsKeys[conversantID] = conversantKey;
                        }
                        if (!stats.actorStats.TryGetValue(conversantKey, out var conversantStats))
                        {
                            conversantStats = new ActorStats();
                            stats.actorStats[conversantKey] = conversantStats;
                        }
                        conversantStats.conversationIDs.Add(conversation.id);

                        // Entry info:
                        stats.numDialogueEntries++;
                        var menuText = entry.MenuText;
                        var dialogueText = entry.DialogueText;
                        if (!(string.IsNullOrEmpty(menuText) && string.IsNullOrEmpty(dialogueText)))
                        {
                            stats.numDialogueEntriesNonBlank++;
                        }
                        var wordCount = GetWordCount(menuText) + GetWordCount(dialogueText);
                        stats.conversationWordCount += wordCount;
                        actorStats.numWords += wordCount;
                        foreach (var field in entry.fields)
                        {
                            if (field.type == FieldType.Localization)
                            {
                                var fieldWordCount = GetWordCount(field.value);
                                stats.conversationWordCount += fieldWordCount;
                                actorStats.numWords += fieldWordCount;
                            }
                        }
                        if (!string.IsNullOrEmpty(entry.sceneEventGuid))
                        {
                            stats.numSceneEvents++;
                        }
                    }
                }
                stats.totalWordCount = stats.questWordCount + stats.conversationWordCount;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static char[] wordDelimiters = new char[] { ' ', '\r', '\n' };

        private int GetWordCount(string s)
        {
            return s.Split(wordDelimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }

    }
}
