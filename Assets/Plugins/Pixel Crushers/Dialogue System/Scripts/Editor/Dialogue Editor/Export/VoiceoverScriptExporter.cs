// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This part of the Dialogue Editor window contains the voiceover script export code.
    /// </summary>
    public static class VoiceoverScriptExporter
    {

        /// <summary>
        /// The main export method. Exports voiceover scripts to a CSV file for each language.
        /// </summary>
        /// <param name="database">Source database.</param>
        /// <param name="filename">Target CSV filename.</param>
        /// <param name="exportActors">If set to <c>true</c> export actors.</param>
        /// <param name="entrytagFormat">Entrytag format, which should match Dialogue Manager config.</param>
        /// <param name="encodingType">Encoding type.</param>
        /// <param name="exportConversationTitleSeparateColumn">Add column for conversation ID.</param>
        public static void Export(DialogueDatabase database, string filename, bool exportActors,
            bool exportConversationTitleSeparateColumn,
            EntrytagFormat entrytagFormat, EncodingType encodingType,
            string voiceoverInfoFieldName)
        {
            if (database == null || string.IsNullOrEmpty(filename)) return;
            var otherLanguages = FindOtherLanguages(database);
            ExportFile(database, string.Empty, filename, exportActors, exportConversationTitleSeparateColumn, entrytagFormat, encodingType, voiceoverInfoFieldName);
            foreach (var language in otherLanguages)
            {
                ExportFile(database, language, filename, exportActors, exportConversationTitleSeparateColumn, entrytagFormat, encodingType, voiceoverInfoFieldName);
            }
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

        private static void ExportFile(DialogueDatabase database, string language, string baseFilename, bool exportActors,
            bool exportConversationTitleSeparateColumn, EntrytagFormat entrytagFormat, EncodingType encodingType,
            string voiceoverInfoFieldName)
        {
            var filename = string.IsNullOrEmpty(language) ? baseFilename
                : Path.GetDirectoryName(baseFilename) + "/" + Path.GetFileNameWithoutExtension(baseFilename) + "_" + language + ".csv";
            using (StreamWriter file = new StreamWriter(filename, false, EncodingTypeTools.GetEncoding(encodingType)))
            {
                ExportDatabaseProperties(database, file);
                if (exportActors) ExportActors(database, file);
                ExportConversations(database, exportConversationTitleSeparateColumn, language, entrytagFormat, voiceoverInfoFieldName, file);
            }
        }

        private static void ExportDatabaseProperties(DialogueDatabase database, StreamWriter file)
        {
            file.WriteLine("Database," + CleanField(database.name));
            file.WriteLine("Author," + CleanField(database.author));
            file.WriteLine("Version," + CleanField(database.version));
            file.WriteLine("Description," + CleanField(database.description));
        }

        private static void ExportActors(DialogueDatabase database, StreamWriter file)
        {
            file.WriteLine(string.Empty);
            file.WriteLine("---Actors---");
            file.WriteLine("Name,Description");
            foreach (var actor in database.actors)
            {
                file.WriteLine(CleanField(actor.Name) + "," + CleanField(actor.LookupValue("Description")));
            }
        }

        private static void ExportConversations(DialogueDatabase database, bool exportConversationTitleSeparateColumn,
            string language, EntrytagFormat entrytagFormat, string voiceoverInfoFieldName, StreamWriter file)
        {
            file.WriteLine(string.Empty);
            file.WriteLine("---Conversations---");

            // Cache actor names:
            Dictionary<int, string> actorNames = new Dictionary<int, string>();

            // Export all conversations:
            foreach (var conversation in database.conversations)
            {
                var conversationTitle = CleanField(conversation.Title);
                file.WriteLine(string.Empty);
                file.WriteLine(string.Format("Conversation {0},{1}", conversation.id, conversationTitle));
                file.WriteLine(string.Format("Description,{0}", CleanField(conversation.Description)));
                StringBuilder sb = new StringBuilder(
                    exportConversationTitleSeparateColumn ? "entrytag,Actor,Conversation,Description,"
                    : "entrytag,Actor,Description,");
                sb.Append(string.IsNullOrEmpty(language) ? "Dialogue Text" : CleanField(language));
                if (!string.IsNullOrEmpty(voiceoverInfoFieldName))
                {
                    sb.AppendFormat(",{0}", CleanField(voiceoverInfoFieldName));
                }
                file.WriteLine(sb.ToString());
                foreach (var entry in conversation.dialogueEntries)
                {
                    if (entry.id > 0)
                    {
                        sb = new StringBuilder();
                        if (!actorNames.ContainsKey(entry.ActorID))
                        {
                            Actor actor = database.GetActor(entry.ActorID);
                            actorNames.Add(entry.ActorID, (actor != null) ? CleanField(actor.Name) : "ActorNotFound");
                        }
                        string actorName = actorNames[entry.ActorID];
                        string description = Field.LookupValue(entry.fields, "Description");
                        string entrytag = database.GetEntrytag(conversation, entry, entrytagFormat);
                        var lineText = string.IsNullOrEmpty(language) ? entry.subtitleText : Field.LookupValue(entry.fields, language);
                        if (exportConversationTitleSeparateColumn)
                        {
                            sb.AppendFormat("{0},{1},{2},{3},{4}", CleanField(entrytag), CleanField(actorName), conversationTitle, CleanField(description), CleanField(lineText));
                        }
                        else
                        {
                            sb.AppendFormat("{0},{1},{2},{3}", CleanField(entrytag), CleanField(actorName), CleanField(description), CleanField(lineText));
                        }
                        if (!string.IsNullOrEmpty(voiceoverInfoFieldName))
                        {
                            var fieldValue = Field.LookupValue(entry.fields, voiceoverInfoFieldName);
                            if (fieldValue == null) fieldValue = string.Empty;
                            sb.AppendFormat(",{0}", fieldValue);
                        }
                        file.WriteLine(sb.ToString());
                    }
                }
            }
        }

        private static string CleanField(string s)
        {
            return CSVExporter.CleanField(s);
        }

        #region Import (Update Info Field)

        public static void Import(DialogueDatabase database, string filename, bool exportActors,
            bool exportConversationTitleSeparateColumn,
            EntrytagFormat entrytagFormat, EncodingType encodingType,
            string voiceoverInfoFieldName)
        {
            if (string.IsNullOrEmpty(voiceoverInfoFieldName)) return;
            if (database == null || string.IsNullOrEmpty(filename)) return;

            try
            {
                // Load CSV:
                var csv = CSVUtility.ReadCSVFile(filename, encodingType);

                // Create dictionary of <entrytag,entry>:
                var dict = new Dictionary<string, DialogueEntry>();
                foreach (var conversation in database.conversations)
                {
                    foreach (var entry in conversation.dialogueEntries)
                    {
                        var entrytag = database.GetEntrytag(conversation, entry, entrytagFormat);
                        dict[entrytag] = entry;
                    }
                }

                // Determine field type from template:
                // (Failing that, use type of first instance of field encountered.)
                var foundFieldType = false;
                var fieldType = FieldType.Text;
                var template = JsonUtility.FromJson<Template>(database.templateJson);
                if (template != null)
                {
                    var templateField = Field.Lookup(template.dialogueEntryFields, voiceoverInfoFieldName);
                    if (templateField != null)
                    {
                        fieldType = templateField.type;
                        foundFieldType = true;
                    }
                }

                // Match lines to entrytag keys:
                foreach (var row in csv)
                {
                    if (row == null || row.Count < 2 || string.IsNullOrEmpty(row[0])) continue;
                    var infoFieldValue = row[row.Count - 1];
                    if (dict.TryGetValue(row[0], out DialogueEntry entry))
                    {
                        var field = Field.Lookup(entry.fields, voiceoverInfoFieldName);
                        if (field != null)
                        {
                            field.value = infoFieldValue;
                            if (!foundFieldType)
                            {
                                foundFieldType = true;
                                fieldType = field.type;
                            }
                        }
                        else if (!string.IsNullOrEmpty(infoFieldValue))
                        {
                            entry.fields.Add(new Field(voiceoverInfoFieldName, infoFieldValue, fieldType));
                        }
                    }
                }
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog("Import Complete", $"{voiceoverInfoFieldName} fields in dialogue entries were updated from {filename}.", "OK");
#endif
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                Debug.LogWarning($"Failed to import info fields from {filename}.");
                return;
            }
        }

        #endregion

    }

}
