// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

            public Dictionary<string, int> questWordCountByLanguage = new Dictionary<string, int>();
            public Dictionary<string, int> conversationWordCountByLanguage = new Dictionary<string, int>();
            public Dictionary<string, int> totalWordCountByLanguage = new Dictionary<string, int>();

            public bool actorStatsFoldout = false;
            public Dictionary<string, ActorStats> actorStats = new Dictionary<string, ActorStats>();
        }

        public class ActorStats
        {
            public HashSet<int> conversationIDs = new HashSet<int>();
            public int numDialogueEntries = 0;
            public int numWords = 0;
            public Dictionary<string, int> numWordsByLanguage = new Dictionary<string, int>();
        }

        private DatabaseStats stats = new DatabaseStats();
        private Dictionary<int, string> actorStatsKeys = new Dictionary<int, string>();

        private const string DefaultLanguage = "Default";

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
                EditorGUI.indentLevel++;
                foreach (var kvp in stats.questWordCountByLanguage)
                {
                    EditorGUILayout.IntField(kvp.Key, kvp.Value);
                }
                EditorGUI.indentLevel--;

                EditorGUILayout.IntField("Conversations", stats.conversationWordCount);
                EditorGUI.indentLevel++;
                foreach (var kvp in stats.conversationWordCountByLanguage)
                {
                    EditorGUILayout.IntField(kvp.Key, kvp.Value);
                }
                EditorGUI.indentLevel--;

                EditorGUILayout.IntField("Total", stats.totalWordCount);
                EditorGUI.indentLevel++;
                foreach (var kvp in stats.totalWordCountByLanguage)
                {
                    EditorGUILayout.IntField(kvp.Key, kvp.Value);
                }
                EditorGUI.indentLevel--;

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
                var actorName = kvp.Key;
                var actorStats = kvp.Value;
                EditorGUILayout.LabelField(actorName);
                EditorGUI.indentLevel++;
                EditorGUILayout.IntField("Conversations", actorStats.conversationIDs.Count);
                EditorGUILayout.IntField("Dialogue Entries", actorStats.numDialogueEntries);
                EditorGUILayout.IntField("Words", actorStats.numWords);
                EditorGUI.indentLevel++;
                foreach (var kvp2 in actorStats.numWordsByLanguage)
                {
                    EditorGUILayout.IntField(kvp2.Key, kvp2.Value);
                }
                EditorGUI.indentLevel--;
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
                RebuildLanguageListFromDatabase();

                stats.questWordCountByLanguage.Clear();
                stats.conversationWordCountByLanguage.Clear();
                stats.totalWordCountByLanguage.Clear();
                stats.questWordCountByLanguage[DefaultLanguage] = 0;
                stats.conversationWordCountByLanguage[DefaultLanguage] = 0;
                stats.totalWordCountByLanguage[DefaultLanguage] = 0;
                foreach (var language in languages)
                {
                    stats.questWordCountByLanguage[language] = 0;
                    stats.conversationWordCountByLanguage[language] = 0;
                    stats.totalWordCountByLanguage[language] = 0;
                }

                stats.numDialogueEntries = stats.numDialogueEntriesNonBlank = stats.numSceneEvents = 0;
                stats.questWordCount = 0;
                stats.conversationWordCount = 0;
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
                        var wordCount = GetWordCount(field.value);
                        stats.questWordCount += wordCount;
                        var language = GetStatsLanguageFromField(field);
                        stats.questWordCountByLanguage[language] += wordCount;
                        stats.totalWordCountByLanguage[language] += wordCount;
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
                            actorStats = CreateActorStats(actorKey);
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
                            conversantStats = CreateActorStats(conversantKey);
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
                        stats.conversationWordCountByLanguage[DefaultLanguage] += wordCount;
                        stats.totalWordCountByLanguage[DefaultLanguage] += wordCount;
                        actorStats.numWords += wordCount;
                        actorStats.numWordsByLanguage[DefaultLanguage] += wordCount;
                        foreach (var field in entry.fields)
                        {
                            if (field.type == FieldType.Localization && !string.IsNullOrEmpty(field.value))
                            {
                                var fieldWordCount = GetWordCount(field.value);
                                var language = GetStatsLanguageFromField(field);
                                stats.conversationWordCountByLanguage[language] += fieldWordCount;
                                stats.totalWordCountByLanguage[language] += fieldWordCount;
                                actorStats.numWords += fieldWordCount;
                                actorStats.numWordsByLanguage[language] += fieldWordCount;
                            }
                        }
                        if (!string.IsNullOrEmpty(entry.sceneEventGuid))
                        {
                            stats.numSceneEvents++;
                        }
                        stats.conversationWordCount = 0;
                        foreach (var kvp in stats.conversationWordCountByLanguage)
                        {
                            int languageWordCount = kvp.Value;
                            stats.conversationWordCount += languageWordCount;
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

        private ActorStats CreateActorStats(string actorKey)
        {
            var actorStats = new ActorStats();
            actorStats.numWordsByLanguage[DefaultLanguage] = 0;
            foreach (var language in languages)
            {
                actorStats.numWordsByLanguage[language] = 0;
            }
            return actorStats;
        }

        private string GetStatsLanguageFromField(Field field)
        {
            return field.type == FieldType.Localization
                ? GetLanguageFromFieldTitle(field.title)
                : DefaultLanguage;
        }

        private static char[] wordDelimiters = new char[] { ' ', '\r', '\n' };

        private int GetWordCount(string s)
        {
            return s.Split(wordDelimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }

    }
}
