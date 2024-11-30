using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Imports CSV files created by Dialogue Editor's Database section > Localization Export/Import.
    /// Can be used at runtime.
    /// </summary>
    public class DialogueDatabaseLocalizationImporter
    {

        [Serializable]
        public class LocalizationLanguages
        {
            public List<string> languages = new List<string>();
            public List<string> extraEntryFields = new List<string>();
            public List<string> extraQuestFields = new List<string>();
            public List<string> extraItemFields = new List<string>();
            public int importMainTextIndex = -1;
            public string outputFolder;
        }

        private LocalizationLanguages localizationLanguages = new LocalizationLanguages();
        private string localizationKeyField = "Articy Id";
        private Dictionary<string, int> conversationIDCache = new Dictionary<string, int>();
        private Conversation lastCachedConversation = null;

        /// <summary>
        /// Imports localization CSV files from specified folder.
        /// </summary>
        /// <param name="database">Dialogue database to put localizations into.</param>
        /// <param name="folderName">Path to CSV files.</param>
        /// <param name="localizationLanguages">Info about languages to import.</param>
        /// <param name="exportLocalizationConversationTitle">If true, first column is conversation Title instead of ID.</param>
        /// <param name="exportLocalizationCreateNewFields">If true, create new field if dialogue entry doesn't have field.</param>
        /// <param name="localizationKeyField">If not blank, use this key field (e.g., "Articy Id") instead of entry ID etc.</param>
        public void ImportLocalizationFilesFromFolder(
            DialogueDatabase database,
            string folderName,
            LocalizationLanguages localizationLanguages,
            bool exportLocalizationConversationTitle = false,
            bool exportLocalizationCreateNewFields = false,
            string localizationKeyField = null)
        {
            this.localizationLanguages = localizationLanguages;
            this.localizationKeyField = localizationKeyField;
            var exportLocalizationKeyField = !string.IsNullOrEmpty(localizationKeyField);
            conversationIDCache.Clear();
            lastCachedConversation = null;
            localizationLanguages.outputFolder = folderName;
            conversationIDCache.Clear();
            lastCachedConversation = null;
            var numLanguages = localizationLanguages.languages.Count;
            for (int i = 0; i < numLanguages; i++)
            {
                var progress = (float)i / (float)numLanguages;
                var language = localizationLanguages.languages[i];
                var alsoImportMainText = localizationLanguages.importMainTextIndex == i;

                //--- Future: Use this class in DialogueEditorWindowLocalization w/callback here.
                //if (EditorUtility.DisplayCancelableProgressBar("Importing Localization CSV", "Importing CSV files for " + language, progress))
                //{
                //    return;
                //}

                // Read actors CSV file:
                var filename = localizationLanguages.outputFolder + "/Actors_" + language + ".csv";
                var lines = ReadCSV(filename);
                CombineMultilineCSVSourceLines(lines);
                for (int j = 2; j < lines.Count; j++)
                {
                    var columns = GetCSVColumnsFromLine(lines[j]);
                    if (columns.Count < 3)
                    {
                        Debug.LogError(filename + ":" + (j + 1) + " Invalid line: " + lines[j]);
                    }
                    else
                    {
                        var actorName = columns[0].Trim();
                        var actorDisplayName = columns[1].Trim();
                        var translatedName = columns[2].Trim();
                        var actor = database.GetActor(actorName);
                        if (actor == null)
                        {
                            Debug.LogError(filename + ": No actor in database is named '" + actorName + "'.");
                            continue;
                        }
                        Field.SetValue(actor.fields, "Display Name " + language, translatedName);
                        if (alsoImportMainText && !string.IsNullOrEmpty(actorDisplayName))
                        {
                            Field.SetValue(actor.fields, "Display Name", actorDisplayName);
                        }
                    }
                }

                // Read dialogue CSV file:
                filename = localizationLanguages.outputFolder + "/Dialogue_" + language + ".csv";
                lines = ReadCSV(filename);
                CombineMultilineCSVSourceLines(lines);
                for (int j = 2; j < lines.Count; j++)
                {
                    var columns = GetCSVColumnsFromLine(lines[j]);
                    if (columns.Count < 7)
                    {
                        Debug.LogError(filename + ":" + (j + 1) + " Invalid line: " + lines[j]);
                    }
                    else
                    {
                        // Peel key field value off front if exporting key fields:
                        string keyFieldValue = null;
                        if (exportLocalizationKeyField)
                        {
                            keyFieldValue = columns[0];
                            columns.RemoveAt(0);
                        }

                        // Get conversation ID:
                        int conversationID = 0;
                        if (exportLocalizationConversationTitle)
                        {
                            var conversationTitle = columns[0];
                            if (!conversationIDCache.ContainsKey(conversationTitle))
                            {
                                var conversation = database.GetConversation(columns[0]);
                                if (conversation == null)
                                {
                                    Debug.LogError(filename + ":" + (j + 1) + " Database doesn't contain conversation '" + columns[0] + "'.");
                                    continue;
                                }
                                conversationIDCache[conversationTitle] = conversation.id;
                            }
                            conversationID = conversationIDCache[conversationTitle];
                        }
                        else
                        {
                            conversationID = Tools.StringToInt(columns[0]);
                        }

                        var entryID = Tools.StringToInt(columns[1]);
                        //columns[2] is Actor. Ignore it.
                        DialogueEntry entry = null;
                        if (exportLocalizationKeyField)
                        {
                            // Find entry by key field:
                            if (lastCachedConversation == null || lastCachedConversation.id != conversationID)
                            {
                                lastCachedConversation = database.GetConversation(conversationID);
                            }
                            entry = lastCachedConversation.dialogueEntries.Find(x => Field.LookupValue(x.fields, localizationKeyField) == keyFieldValue);
                        }
                        else
                        {
                            // Find entry by ID:
                            entry = database.GetDialogueEntry(conversationID, entryID);
                            if (entry == null)
                            {
                                Debug.LogError(filename + ":" + (j + 1) + " Database doesn't contain conversation " + conversationID + " dialogue entry " + entryID);
                            }
                        }

                        // If we found the entry, update its fields:
                        if (entry != null)
                        {
                            Field.SetValue(entry.fields, language, columns[4], FieldType.Localization);
                            Field.SetValue(entry.fields, "Menu Text " + language, columns[6], FieldType.Localization);

                            // Check if we also need to import updated main text.
                            if (alsoImportMainText)
                            {
                                entry.DialogueText = columns[3];
                                entry.MenuText = columns[5];
                            }

                            // Extra entry fields:
                            for (int k = 0; k < localizationLanguages.extraEntryFields.Count; k++)
                            {
                                var field = localizationLanguages.extraEntryFields[k];
                                int columnIndex = 8 + (k * 2) + 1;
                                if (string.IsNullOrEmpty(field)) continue;

                                if (!exportLocalizationCreateNewFields &&
                                    !Field.FieldExists(entry.fields, field) &&
                                    string.IsNullOrEmpty(columns[columnIndex - 1]))
                                {
                                    continue;
                                }

                                Field.SetValue(entry.fields, field + " " + language, columns[columnIndex]);

                                if (alsoImportMainText)
                                {
                                    Field.SetValue(entry.fields, field, columns[columnIndex - 1]);
                                }
                            }
                        }
                    }
                }

                // Read quests CSV file:
                filename = localizationLanguages.outputFolder + "/Quests_" + language + ".csv";
                if (File.Exists(filename))
                {
                    lines = ReadCSV(filename);
                    CombineMultilineCSVSourceLines(lines);
                    for (int j = 2; j < lines.Count; j++)
                    {
                        var columns = GetCSVColumnsFromLine(lines[j]);
                        if (columns.Count < 11)
                        {
                            Debug.LogError(filename + ":" + (j + 1) + " Invalid line: " + lines[j]);
                        }
                        else
                        {
                            var quest = database.GetItem(columns[0]);
                            if (quest == null)
                            {
                                // Skip if quest is not present.
                            }
                            else
                            {
                                var displayName = columns[1];
                                var translatedDisplayName = columns[2];
                                if (!string.IsNullOrEmpty(translatedDisplayName))
                                {
                                    if (!quest.FieldExists("Display Name")) Field.SetValue(quest.fields, "Display Name", displayName);
                                    Field.SetValue(quest.fields, "Display Name " + language, translatedDisplayName, FieldType.Localization);
                                }
                                var group = columns[3];
                                var translatedGroup = columns[4];
                                var needToAddGroup = !quest.FieldExists("Group") && (!string.IsNullOrEmpty(group) || !string.IsNullOrEmpty(translatedGroup));
                                if (quest.FieldExists("Group") && string.IsNullOrEmpty(quest.LookupValue("Group")) && !string.IsNullOrEmpty(group)) needToAddGroup = true;
                                if (needToAddGroup) Field.SetValue(quest.fields, "Group", group);
                                if (quest.FieldExists("Group")) Field.SetValue(quest.fields, "Group " + language, translatedGroup, FieldType.Localization);
                                Field.SetValue(quest.fields, "Description " + language, columns[6], FieldType.Localization);
                                Field.SetValue(quest.fields, "Success Description " + language, columns[8], FieldType.Localization);
                                Field.SetValue(quest.fields, "Failure Description " + language, columns[10], FieldType.Localization);

                                // Extra quest fields:
                                int numExtraQuestFields = 0;
                                for (int k = 0; k < localizationLanguages.extraQuestFields.Count; k++)
                                {
                                    var field = localizationLanguages.extraQuestFields[k];
                                    if (string.IsNullOrEmpty(field)) continue;

                                    int columnIndex = 11 + (k * 2) + 1;
                                    numExtraQuestFields++;

                                    if (!exportLocalizationCreateNewFields &&
                                        !Field.FieldExists(quest.fields, field) &&
                                        string.IsNullOrEmpty(columns[columnIndex - 1]))
                                    {
                                        continue;
                                    }

                                    Field.SetValue(quest.fields, field + " " + language, columns[columnIndex]);

                                    if (alsoImportMainText)
                                    {
                                        Field.SetValue(quest.fields, field, columns[columnIndex - 1]);
                                    }
                                }

                                // Quest entry fields:
                                var entryCount = quest.LookupInt("Entry Count");
                                for (int k = 0; k < entryCount; k++)
                                {
                                    var index = 12 + (2 * numExtraQuestFields) + (k * 2);
                                    Field.SetValue(quest.fields, "Entry " + (k + 1) + " " + language, columns[index], FieldType.Localization);
                                }

                                // Check if we need to also import main text:
                                if (alsoImportMainText)
                                {
                                    if (quest.FieldExists("Display Name")) Field.SetValue(quest.fields, "Display Name", displayName);
                                    if (quest.FieldExists("Group")) Field.SetValue(quest.fields, "Group", group, FieldType.Text);
                                    Field.SetValue(quest.fields, "Description", columns[5], FieldType.Text);
                                    Field.SetValue(quest.fields, "Success Description", columns[7], FieldType.Text);
                                    Field.SetValue(quest.fields, "Failure Description", columns[9], FieldType.Text);
                                    for (int k = 0; k < entryCount; k++)
                                    {
                                        Field.SetValue(quest.fields, "Entry " + (k + 1), columns[11 + 2 * k], FieldType.Text);
                                    }
                                }
                            }
                        }
                    }
                }

                // Read items CSV file:
                filename = localizationLanguages.outputFolder + "/Items_" + language + ".csv";
                if (File.Exists(filename))
                {
                    lines = ReadCSV(filename);
                    CombineMultilineCSVSourceLines(lines);
                    for (int j = 2; j < lines.Count; j++)
                    {
                        var columns = GetCSVColumnsFromLine(lines[j]);
                        if (columns.Count < 5)
                        {
                            Debug.LogError(filename + ":" + (j + 1) + " Invalid line: " + lines[j]);
                        }
                        else
                        {
                            var item = database.GetItem(columns[0]);
                            if (item == null)
                            {
                                // Skip if item is not present.
                            }
                            else
                            {
                                var displayName = columns[1];
                                var translatedDisplayName = columns[2];
                                if (!string.IsNullOrEmpty(translatedDisplayName))
                                {
                                    if (!item.FieldExists("Display Name")) Field.SetValue(item.fields, "Display Name", displayName);
                                    Field.SetValue(item.fields, "Display Name " + language, translatedDisplayName, FieldType.Localization);
                                }
                                Field.SetValue(item.fields, "Description " + language, columns[4], FieldType.Localization);

                                // Extra item fields:
                                int numExtraItemFields = 0;
                                for (int k = 0; k < localizationLanguages.extraItemFields.Count; k++)
                                {
                                    var field = localizationLanguages.extraItemFields[k];
                                    if (string.IsNullOrEmpty(field)) continue;

                                    int columnIndex = 4 + (k * 2) + 1;
                                    numExtraItemFields++;

                                    if (!exportLocalizationCreateNewFields &&
                                        !Field.FieldExists(item.fields, field) &&
                                        string.IsNullOrEmpty(columns[columnIndex - 1]))
                                    {
                                        continue;
                                    }

                                    Field.SetValue(item.fields, field + " " + language, columns[columnIndex]);

                                    if (alsoImportMainText)
                                    {
                                        Field.SetValue(item.fields, field, columns[columnIndex - 1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<string> GetCSVColumnsFromLine(string line)
        {
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
            List<string> values = new List<string>();
            foreach (Match match in csvSplit.Matches(line))
            {
                values.Add(UnwrapCSVValue(match.Value.TrimStart(',')));
            }
            return values;
        }

        private List<string> ReadCSV(string filename)
        {
            var lines = new List<string>();
            StreamReader sr = new StreamReader(filename, new UTF8Encoding(true));
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line.TrimEnd());
            }
            sr.Close();
            return lines;
        }

        /// <summary>
        /// Combines lines that are actually a multiline CSV row. This also helps prevent the 
        /// CSV-splitting regex from hanging due to catastrophic backtracking on unterminated quotes.
        /// </summary>
        private void CombineMultilineCSVSourceLines(List<string> sourceLines)
        {
            int lineNum = 0;
            int safeguard = 0;
            int MaxIterations = 999999;
            while ((lineNum < sourceLines.Count) && (safeguard < MaxIterations))
            {
                safeguard++;
                string line = sourceLines[lineNum];
                if (line == null)
                {
                    sourceLines.RemoveAt(lineNum);
                }
                else
                {
                    bool terminated = true;
                    char previousChar = (char)0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        char currentChar = line[i];
                        bool isQuote = (currentChar == '"') && (previousChar != '\\');
                        if (isQuote) terminated = !terminated;
                        previousChar = currentChar;
                    }
                    if (terminated || (lineNum + 1) >= sourceLines.Count)
                    {
                        if (!terminated) sourceLines[lineNum] = line + '"';
                        lineNum++;
                    }
                    else
                    {
                        sourceLines[lineNum] = line + "\\n" + sourceLines[lineNum + 1];
                        sourceLines.RemoveAt(lineNum + 1);
                    }
                }
            }
        }

        string UnwrapCSVValue(string s)
        {
            string s2 = s.Replace("\\n", "\n").Replace("\\r", "\r");
            if (s2.StartsWith("\"") && s2.EndsWith("\""))
            {
                s2 = s2.Substring(1, s2.Length - 2).Replace("\"\"", "\"");
            }
            return s2;
        }

    }

}
