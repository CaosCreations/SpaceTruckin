// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This part of the Dialogue Editor window contains the proofreading export code.
    /// </summary>
    public static class ProofreadingExporter
    {

        private const string Head = "|>";

        #region Export

        /// <summary>
        /// The main export method. Exports a proofreading text file for each language.
        /// </summary>
        /// <param name="database">Source database.</param>
        /// <param name="filename">Target CSV filename.</param>
        public static void Export(DialogueDatabase database, string filename, EncodingType encodingType)
        {
            if (database == null || string.IsNullOrEmpty(filename)) return;
            var otherLanguages = FindOtherLanguages(database);
            Localization.language = string.Empty;
            ExportFile(database, string.Empty, filename, encodingType);
            foreach (var language in otherLanguages)
            {
                Localization.language = language;
                ExportFile(database, language, filename, encodingType);
            }
            Localization.language = string.Empty;
        }

        private static List<string> FindOtherLanguages(DialogueDatabase database)
        {
            var otherLanguages = new List<string>();
            foreach (var conversation in database.conversations)
            {
                foreach (var entry in conversation.dialogueEntries)
                {
                    foreach (var field in entry.fields)
                    {
                        if ((field.type == FieldType.Localization) &&
                            !otherLanguages.Contains(field.title) &&
                            !field.title.Contains(" "))
                        {
                            otherLanguages.Add(field.title);
                        }
                    }
                }
            }
            return otherLanguages;
        }

        private static void ExportFile(DialogueDatabase database, string language, string baseFilename, EncodingType encodingType)
        {
            var filename = string.IsNullOrEmpty(language) ? baseFilename
                : Path.GetDirectoryName(baseFilename) + "/" + Path.GetFileNameWithoutExtension(baseFilename) + "_" + language + ".txt";
            using (StreamWriter file = new StreamWriter(filename, false, EncodingTypeTools.GetEncoding(encodingType)))
            {
                ExportHeading(database, file);
                ExportActors(database, file, language);
                ExportQuests(database, file, language);
                ExportConversations(database, file, language);
            }
        }

        private static void ExportHeading(DialogueDatabase database, StreamWriter file)
        {
            file.WriteLine($"{Head} Proofreading File for {database.name}");
            file.WriteLine($"{Head} Do not modify lines that start with '{Head}'!");
            file.WriteLine(Head);
        }

        private static void ExportLineIfSet(StreamWriter file, string heading, string line)
        {
            if (string.IsNullOrEmpty(line)) return;
            file.WriteLine($"{Head} {heading}:");
            file.WriteLine(line);
        }

        private static string GetLocalizedValue(List<Field> fields, string fieldTitle)
        {
            if (Localization.isDefaultLanguage)
            {
                return Field.LookupValue(fields, fieldTitle);
            }
            else
            {
                return Field.LookupValue(fields, $"{fieldTitle} {Localization.language}");
            }
        }

        private static void ExportActors(DialogueDatabase database, StreamWriter file, string language)
        {
            file.WriteLine($"{Head} ACTORS -------------------------------------------------");
            foreach (var actor in database.actors)
            {
                file.WriteLine($"{Head} ACTOR [{actor.id}]: {actor.Name}");
                ExportLineIfSet(file, "DISPLAY NAME", GetLocalizedValue(actor.fields, "Display Name"));
                ExportLineIfSet(file, "DESCRIPTION", GetLocalizedValue(actor.fields, "Description"));
                file.WriteLine(Head);
            }
        }

        private static void ExportQuests(DialogueDatabase database, StreamWriter file, string language)
        {
            file.WriteLine($"{Head} QUESTS -------------------------------------------------");
            foreach (var quest in database.items)
            {
                if (quest.IsItem) continue;
                file.WriteLine($"{Head} QUEST [{quest.id}]: {quest.Name}");
                ExportLineIfSet(file, "DISPLAY NAME", GetLocalizedValue(quest.fields, "Display Name"));
                ExportLineIfSet(file, "GROUP", GetLocalizedValue(quest.fields, "Group"));
                ExportLineIfSet(file, "GROUP DISPLAY NAME", GetLocalizedValue(quest.fields, "Group Display Name"));
                ExportLineIfSet(file, "DESCRIPTION", GetLocalizedValue(quest.fields, "Description"));
                ExportLineIfSet(file, "SUCCESS DESCRIPTION", GetLocalizedValue(quest.fields, "Success Description"));
                ExportLineIfSet(file, "FAILURE DESCRIPTION", GetLocalizedValue(quest.fields, "Failure Description"));
                if (quest.FieldExists("Entry Count"))
                {
                    var entryCount = quest.LookupInt("Entry Count");
                    for (int i = 1; i <= entryCount; i++)
                    {
                        ExportLineIfSet(file, $"ENTRY {i}", GetLocalizedValue(quest.fields, $"Entry {i}"));
                    }
                }
                file.WriteLine(Head);
            }
        }

        private static void ExportConversations(DialogueDatabase database, StreamWriter file, string language)
        {
            file.WriteLine($"{Head} CONVERSATIONS ------------------------------------------");

            // Cache actor names:
            Dictionary<int, string> actorNames = new Dictionary<int, string>();

            // Export all conversations:
            foreach (var conversation in database.conversations)
            {
                file.WriteLine($"{Head} CONVERSATION [{conversation.id}]: {conversation.Title}");
                ExportLineIfSet(file, "DESCRIPTION", conversation.Description);
                foreach (var entry in conversation.dialogueEntries)
                {
                    if (entry.id == 0) continue;
                    var dialogueText = entry.currentDialogueText;
                    var menuText = entry.currentMenuText;
                    if (string.IsNullOrEmpty(dialogueText) && string.IsNullOrEmpty(menuText)) continue;
                    var actorID = entry.ActorID;
                    string actorName;
                    if (!actorNames.TryGetValue(actorID, out actorName))
                    {
                        var actor = database.GetActor(actorID);
                        actorName = (actor != null) ? actor.Name : $"Actor {actorID}";
                        actorNames.Add(actorID, actorName);
                    }
                    file.WriteLine($"{Head} ENTRY [{entry.id}] ({actorName})");
                    ExportLineIfSet(file, "MENU TEXT", menuText);
                    ExportLineIfSet(file, "DIALOGUE TEXT", dialogueText);
                }
                file.WriteLine(Head);
            }
        }

        #endregion

        #region Import

        private static int numChanges;

        public static void Import(DialogueDatabase database, string filename, EncodingType encodingType)
        {
            if (database == null || string.IsNullOrEmpty(filename)) return;
            var otherLanguages = FindOtherLanguages(database);
            Localization.language = string.Empty;
            numChanges = 0;
            Debug.Log($"Importing proofreading changes from {filename}:");
            ImportFile(database, string.Empty, filename, encodingType);
            foreach (var language in otherLanguages)
            {
                Localization.language = language;
                ImportFile(database, language, filename, encodingType);
            }
            Localization.language = string.Empty;
            Debug.Log($"Import complete. {numChanges} changes imported.");
        }

        private static void ImportFile(DialogueDatabase database, string language, string baseFilename, EncodingType encodingType)
        {
            var filename = string.IsNullOrEmpty(language) ? baseFilename
                : Path.GetDirectoryName(baseFilename) + "/" + Path.GetFileNameWithoutExtension(baseFilename) + "_" + language + ".txt";
            var lines = new Queue<string>(File.ReadAllLines(filename));
            ImportHeading(database, lines);
            ImportActors(database, lines, language);
            ImportQuests(database, lines, language);
            ImportConversations(database, lines, language);
        }

        private static void ImportHeading(DialogueDatabase database, Queue<string> lines)
        {
            lines.Dequeue(); //($"{Head} Proofreading File for {database.name}");
            lines.Dequeue(); //($"{Head} Do not modify lines that start with '{Head}'!");
            lines.Dequeue(); //($"{Head}");
        }

        private static void SkipToNextHead(Queue<string> lines)
        {
            while (lines.Count > 0 && !lines.Peek().StartsWith(Head))
            {
                lines.Dequeue();
            }
        }

        private static void SkipPastNextBlankHead(Queue<string> lines)
        {
            while (lines.Count > 0 && lines.Peek() != Head)
            {
                lines.Dequeue();
            }
            if (lines.Count > 0 && lines.Peek() == Head)
            {
                lines.Dequeue();
            }
        }

        private static string ReadToNextHead(Queue<string> lines)
        {
            string s = string.Empty;
            while (lines.Count > 0 && !lines.Peek().StartsWith(Head))
            {
                if (!string.IsNullOrEmpty(s)) s += "\n";
                s += lines.Dequeue();
            }
            return s;
        }

        private static void TryImportField(Queue<string> lines, string assetIdentifier,
            List<Field> fields, string fieldHeading, string fieldTitle, 
            bool isLocalizationBuiltIntoFieldTitle = false)
        { 
            if (lines.Peek() == $"{Head} {fieldHeading}:")
            {
                lines.Dequeue();
                var newValue = ReadToNextHead(lines);
                SetFieldIfDifferent(assetIdentifier, fields, fieldTitle, newValue, isLocalizationBuiltIntoFieldTitle);
            }
        }

        private static void SetFieldIfDifferent(string assetIdentifier, List<Field> fields,
            string fieldTitle, string newValue, bool isLocalizationBuiltIntoFieldTitle = false)
        {
            if (isLocalizationBuiltIntoFieldTitle)
            {
                var oldValue = Field.LookupValue(fields, fieldTitle);
                if (newValue != oldValue && !string.IsNullOrEmpty(oldValue))
                {
                    Field.SetValue(fields, fieldTitle, newValue);
                    numChanges++;
                    Debug.Log($"{assetIdentifier} {fieldTitle} --> {newValue}");
                }
            }
            else
            {
                var oldValue = Field.LookupLocalizedValue(fields, fieldTitle);
                if (newValue != oldValue)
                {
                    var localizedFieldTitle = GetLocalizedFieldTitle(fieldTitle);
                    Field.SetValue(fields, localizedFieldTitle, newValue);
                    numChanges++;
                    Debug.Log($"{assetIdentifier} {localizedFieldTitle} --> {newValue}");
                }
            }
        }

        private static string GetLocalizedFieldTitle(string fieldTitle)
        {
            return Localization.IsDefaultLanguage
                ? fieldTitle
                : fieldTitle + " " + Localization.language;
        }

        private static void ImportActors(DialogueDatabase database, Queue<string> lines, string language)
        {
            lines.Dequeue(); //($"{Head} ACTORS -------------------------------------------------");
            while (lines.Count > 0 && lines.Peek().StartsWith($"{Head} ACTOR ["))
            {
                var line = lines.Dequeue(); //($"{Head} ACTOR [{actor.id}]: {actor.Name}");
                var leftBracketPos = line.IndexOf('[');
                var rightBracketPos = line.IndexOf("]");
                var actorIDString = line.Substring(leftBracketPos + 1, rightBracketPos - leftBracketPos - 1);
                var actorID = int.Parse(actorIDString);
                var actor = database.GetActor(actorID);
                if (actor == null)
                {
                    Debug.LogWarning($"Dialogue System: Database doesn't have an actor with ID {actorID} ({line})");
                    SkipPastNextBlankHead(lines);
                }
                else
                {
                    var actorIdentifier = $"Actor [{actorID}] ({actor.Name})";
                    TryImportField(lines, actorIdentifier, actor.fields, "DISPLAY NAME", "Display Name");
                    TryImportField(lines, actorIdentifier, actor.fields, "DESCRIPTION", "Description");
                    lines.Dequeue(); //{Head}
                }
            }
        }

        private static void ImportQuests(DialogueDatabase database, Queue<string> lines, string language)
        {
            lines.Dequeue(); //($"{Head} QUESTS -------------------------------------------------");
            while (lines.Count > 0 && lines.Peek().StartsWith($"{Head} QUEST ["))
            {
                var line = lines.Dequeue(); //($"{Head} QUEST [{quest.id}]: {quest.Name}");
                var leftBracketPos = line.IndexOf('[');
                var rightBracketPos = line.IndexOf("]");
                var questID = int.Parse(line.Substring(leftBracketPos + 1, rightBracketPos - leftBracketPos - 1));
                var quest = database.GetItem(questID);
                if (quest == null)
                {
                    Debug.LogWarning($"Dialogue System: Database doesn't have a quest with ID {questID} ({line})");
                    SkipPastNextBlankHead(lines);
                }
                else
                {
                    var questIdentifier = $"Quest[{questID}] ({quest.Name})";
                    TryImportField(lines, questIdentifier, quest.fields, "DISPLAY NAME", "Display Name");
                    TryImportField(lines, questIdentifier, quest.fields, "GROUP", "Group");
                    TryImportField(lines, questIdentifier, quest.fields, "GROUP DISPLAY NAME", "Group Display Name");
                    TryImportField(lines, questIdentifier, quest.fields, "DESCRIPTION", "Description");
                    TryImportField(lines, questIdentifier, quest.fields, "SUCCESS DESCRIPTION", "Success Description");
                    TryImportField(lines, questIdentifier, quest.fields, "FAILURE DESCRIPTION", "Failure Description");
                    while (lines.Count > 0 && lines.Peek().StartsWith($"{Head} ENTRY"))
                    {
                        line = lines.Dequeue();
                        var entryIDString = line.Substring(line.LastIndexOf(" ") + 1);
                        entryIDString = entryIDString.Substring(0, entryIDString.Length - 1);
                        if (int.TryParse(entryIDString, out var entryNum))
                        {
                            var newEntryText = ReadToNextHead(lines);
                        }
                    }
                    lines.Dequeue(); //{Head}
                }
            }
        }

        private static void ImportConversations(DialogueDatabase database, Queue<string> lines, string language)
        {
            lines.Dequeue(); //($"{Head} CONVERSATIONS ------------------------------------------");
            while (lines.Count > 0 && lines.Peek().StartsWith($"{Head} CONVERSATION ["))
            {
                var line = lines.Dequeue(); //($"{Head} CONVERSATION [{conversation.id}]: {conversation.Title}");
                var leftBracketPos = line.IndexOf('[');
                var rightBracketPos = line.IndexOf("]");
                var conversationID = int.Parse(line.Substring(leftBracketPos + 1, rightBracketPos - leftBracketPos - 1));
                var conversation = database.GetConversation(conversationID);
                if (conversation == null)
                {
                    Debug.LogWarning($"Dialogue System: Database doesn't have an actor with ID {conversationID} ({line})");
                    SkipPastNextBlankHead(lines);
                }
                else
                {
                    var conversationTitle = conversation.Title;
                    var conversationIdentifier = $"Conversation [{conversationID}] ({conversationTitle})";
                    TryImportField(lines, conversationIdentifier, conversation.fields, "DESCRIPTION", "Description");
                    var menuTextTitle = Localization.isDefaultLanguage ? "Menu Text" : $"Menu Text {Localization.language}";
                    var dialogueTextTitle = Localization.isDefaultLanguage ? "Dialogue Text" : Localization.language;
                    while (lines.Count > 0 && lines.Peek().StartsWith($"{Head} ENTRY ["))
                    {
                        line = lines.Dequeue();
                        leftBracketPos = line.IndexOf('[');
                        rightBracketPos = line.IndexOf("]");
                        var entryID = int.Parse(line.Substring(leftBracketPos + 1, rightBracketPos - leftBracketPos - 1));
                        var entry = conversation.GetDialogueEntry(entryID);
                        if (entry == null)
                        {
                            Debug.LogWarning($"Dialogue System: Database doesn't have a conversation with ID {conversationID} ({line})");
                        }
                        else
                        {
                            var entryIdentifier = $"Conversation '{conversationTitle}' Entry [{entryID}]";
                            TryImportField(lines, entryIdentifier, entry.fields, "MENU TEXT", menuTextTitle, true);
                            TryImportField(lines, entryIdentifier, entry.fields, "DIALOGUE TEXT", dialogueTextTitle, true);
                        }
                    }
                    lines.Dequeue(); //{Head}
                }
            }
        }

    }

    #endregion

}
