// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PixelCrushers.DialogueSystem
{

    public class UniqueIDWindow : EditorWindow
    {

        [MenuItem("Tools/Pixel Crushers/Dialogue System/Tools/Unique ID Tool", false, 3)]
        public static void OpenUniqueIDWindow()
        {
            instance = EditorWindow.GetWindow<UniqueIDWindow>(false, "Unique IDs");
        }

        public static UniqueIDWindow instance = null;

        // Private fields for the window:

        public UniqueIDWindowPrefs prefs = null;
        private Template template = null;

        private bool verbose = false;
        public string report;

        private Vector2 scrollPosition = Vector2.zero;

        #region Initialization

        void OnEnable()
        {
            minSize = new Vector2(340, 128);
            if (prefs == null)
            {
                prefs = UniqueIDWindowPrefs.Load();
                prefs.RelinkDatabases();
            }
            template = TemplateTools.LoadFromEditorPrefs();
        }

        void OnDisable()
        {
            if (prefs != null) prefs.Save();
        }

        #endregion

        #region GUI

        void OnGUI()
        {
            // Validate prefs:
            if (prefs == null) prefs = UniqueIDWindowPrefs.Load();
            if (prefs.databases == null) prefs.databases = new List<DialogueDatabase>();

            // Draw window:
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            try
            {
                EditorGUILayout.HelpBox("This tool reassigns IDs so all Actors, Items, Locations, Conversations have unique IDs. " +
                                        "This allows your project to load multiple dialogue databases at runtime without conflicting IDs. " +
                                        "Actors, Items, and Locations with the same name will be assigned the same ID. " +
                                        "All conversations will have unique IDs, even if two conversations have the same title.",
                                        MessageType.None);
                DrawDatabaseSection();
                DrawButtonSection();
                DrawReport();
            }
            finally
            {
                EditorGUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// Draws the database list section.
        /// </summary>
        private void DrawDatabaseSection()
        {
            DrawDatabaseHeader();
            DrawDatabaseList();
        }

        private void DrawDatabaseHeader()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Dialogue Databases", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("All", EditorStyles.miniButtonLeft, GUILayout.Width(48)))
            {
                if (EditorUtility.DisplayDialog("Add All Databases in Project",
                                                string.Format("Do you want to find and add all dialogue databases in the entire project?", EditorWindowTools.GetCurrentDirectory()),
                                                "Ok", "Cancel"))
                {
                    AddAllDatabasesInProject();
                }
            }
            if (GUILayout.Button("Folder", EditorStyles.miniButtonMid, GUILayout.Width(48)))
            {
                if (EditorUtility.DisplayDialog("Add All Databases in Folder",
                                                string.Format("Do you want to find and add all dialogue databases in the folder {0}?", EditorWindowTools.GetCurrentDirectory()),
                                                "Ok", "Cancel"))
                {
                    AddAllDatabasesInFolder(EditorWindowTools.GetCurrentDirectory(), false);
                }
            }
            if (GUILayout.Button("+", EditorStyles.miniButtonRight, GUILayout.Width(22)))
            {
                prefs.databases.Add(null);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDatabaseList()
        {
            EditorGUI.BeginChangeCheck();
            EditorWindowTools.StartIndentedSection();
            DialogueDatabase databaseToDelete = null;
            for (int i = 0; i < prefs.databases.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                prefs.databases[i] = EditorGUILayout.ObjectField(prefs.databases[i], typeof(DialogueDatabase), false) as DialogueDatabase;
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(22))) databaseToDelete = prefs.databases[i];
                EditorGUILayout.EndHorizontal();

            }
            if (databaseToDelete != null) prefs.databases.Remove(databaseToDelete);
            EditorWindowTools.EndIndentedSection();
            if (EditorGUI.EndChangeCheck()) prefs.Save();
        }

        public void AddAllDatabasesInProject()
        {
            prefs.databases.Clear();
            AddAllDatabasesInFolder("Assets", true);
            prefs.Save();
        }

        public void AddAllDatabasesInFolder(string folderPath, bool recursive)
        {
            string[] filePaths = Directory.GetFiles(folderPath, "*.asset", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (string filePath in filePaths)
            {
                string assetPath = filePath.Replace("\\", "/");
                DialogueDatabase database = AssetDatabase.LoadAssetAtPath(assetPath, typeof(DialogueDatabase)) as DialogueDatabase;
                if ((database != null) && (!prefs.databases.Contains(database)))
                {
                    prefs.databases.Add(database);
                }
            }
            prefs.Save();
        }

        private void DrawButtonSection()
        {
            verbose = EditorGUILayout.Toggle("Verbose Logging", verbose);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawClearButton();
            DrawProcessButton();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the Clear button, and clears the prefs if clicked.
        /// </summary>
        private void DrawClearButton()
        {
            if (GUILayout.Button("Clear Settings"))
            {
                prefs.Clear();
                report = string.Empty;
                UniqueIDWindowPrefs.DeleteEditorPrefs();
            }
        }

        /// <summary>
        /// Draws the Process button, and processes the databases if clicked.
        /// </summary>
        private void DrawProcessButton()
        {
            if (GUILayout.Button("Process")) ProcessDatabases();
        }

        private void DrawReport()
        {
            if (string.IsNullOrEmpty(report)) return;
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Copy To Clipboard"))
            {
                EditorGUIUtility.systemCopyBuffer = report;
                Debug.Log("Copied Unique ID Tool report to system clipboard.");
            }
            if (GUILayout.Button("Clear Report"))
            {
                report = string.Empty;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField(report, EditorStyles.textArea);
        }

        #endregion

        #region Process (Top Level)

        private class IDConversion
        {
            public int oldID = 0;
            public int newID = 0;
            public IDConversion() { }
            public IDConversion(int oldID, int newID)
            {
                this.oldID = oldID;
                this.newID = newID;
            }
        }

        private class MasterIDs
        {
            public Dictionary<string, IDConversion> actors = new Dictionary<string, IDConversion>();
            public Dictionary<string, IDConversion> items = new Dictionary<string, IDConversion>();
            public Dictionary<string, IDConversion> locations = new Dictionary<string, IDConversion>();
            public Dictionary<string, IDConversion> variables = new Dictionary<string, IDConversion>();
            public HashSet<int> usedNewActorIDs = new HashSet<int>();
            public HashSet<int> usedNewItemIDs = new HashSet<int>();
            public HashSet<int> usedNewLocationIDs = new HashSet<int>();
            public HashSet<int> usedNewVariableIDs = new HashSet<int>();
            public HashSet<int> usedNewConversationIDs = new HashSet<int>();
            public int highestActorID = 0;
            public int highestItemID = 0;
            public int highestLocationID = 0;
            public int highestVariableID = 0;
            public int highestConversationID = 0;

            // List of databases. Each one: conversation title -> actor name:
            public List<Dictionary<string, string>> actorNamesInConversations = new List<Dictionary<string, string>>();
            public List<Dictionary<string, string>> conversantNamesInConversations = new List<Dictionary<string, string>>();
        }

        public void ProcessDatabases()
        {
            try
            {
                report = "Unique ID Tool Report:";

                // Get unique list of all databases assigned in window:
                List<DialogueDatabase> distinct = prefs.databases.Distinct().ToList();
                distinct.RemoveAll(x => x == null);
                if (distinct.Count == 0)
                {
                    report += " No databases to process.";
                    return;
                }
                MasterIDs masterIDs = new MasterIDs();
                prefs.databases = distinct;

                // Set BaseIDs for all databases that don't have them set:
                SetBaseIDs(distinct, masterIDs);

                // Sort database list by increasing Base ID so renumbering uses
                // lowest value for each ID:
                distinct.Sort((x, y) => x.baseID.CompareTo(y.baseID));

                // Rebaseline IDs: (renumber asset IDs from each database's Base ID)
                RebaselineIDs(distinct, masterIDs);

                // Determine new IDs:
                for (int i = 0; i < distinct.Count; i++)
                {
                    var database = distinct[i];
                    if (database != null)
                    {
                        EditorUtility.DisplayProgressBar("Processing Databases (Phase 1/2)", database.name, i / prefs.databases.Count);
                        RecordActorConversantNamesPerDatabaseConversation(database, masterIDs);
                        GetNewIDs(database, masterIDs);
                        if (!VerifyUniqueConversationIDs(database)) return;
                    }
                }

                // Apply new IDs:
                for (int i = 0; i < distinct.Count; i++)
                {
                    var database = distinct[i];
                    if (database != null)
                    {
                        EditorUtility.DisplayProgressBar("Processing Databases (Phase 2/2)", database.name, i / prefs.databases.Count);
                        ProcessDatabase(database, masterIDs, i);
                        EditorUtility.SetDirty(database);
                    }
                }
                if (verbose) report += "\n";
                report += "Assigned unique IDs to " + prefs.databases.Count + " databases.";
                AssetDatabase.SaveAssets();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                Debug.Log(report);
            }
        }

        #endregion

        #region Set Base IDs

        private void SetBaseIDs(List<DialogueDatabase> distinct, MasterIDs masterIDs)
        {
            // Determine the next BaseID to use for databases with unassigned BaseIDs:
            int highestBaseID = 0;
            var usedBaseIDs = new HashSet<int>();
            DialogueDatabase databaseWithDefaultBaseID = null;
            foreach (var database in distinct)
            {
                highestBaseID = Mathf.Max(highestBaseID, database.baseID);
                var isDefaultBaseID = (database.baseID <= 1);
                var isNoDefaultBaseID = databaseWithDefaultBaseID == null;
                var isFirstDefaultBaseID = isDefaultBaseID && isNoDefaultBaseID;
                if (isFirstDefaultBaseID)
                {
                    databaseWithDefaultBaseID = database;
                }
                else if (usedBaseIDs.Contains(database.baseID))
                {
                    database.baseID = 0; // Reset so we can set it to a unique value below.
                }
                usedBaseIDs.Add(database.baseID);
            }
            int nextBaseID = ((highestBaseID / 10000) + 1) * 10000;

            // Assign BaseIDs:
            foreach (var database in distinct)
            {
                if (database == databaseWithDefaultBaseID) continue;
                if (database.baseID > 1) continue;
                database.baseID = nextBaseID;
                report += $"\nDatabase {database.name} Base ID-->{database.baseID}";
                nextBaseID += 10000;
            }
        }

        #endregion

        #region Rebaseline IDs

        private void RebaselineIDs(List<DialogueDatabase> distinct, MasterIDs masterIDs)
        {
            foreach (var database in distinct)
            {
                RebaselineIDsInDatabase(database);
            }
        }

        private void RebaselineIDsInDatabase(DialogueDatabase database)
        {
            RebaselineActorIDs(database);
            RebaselineItemIDs(database);
            RebaselineLocationIDs(database);
            RebaselineVariableIDs(database);
            //RebaselineConversationIDs(database);
        }

        private void RebaselineActorIDs(DialogueDatabase database)
        {
            // Determine new IDs:
            var newIDs = new Dictionary<int, int>();
            var nextID = database.baseID;
            foreach (var actor in database.actors)
            {
                if (actor.id != nextID)
                {
                    if (verbose) report += $"\nRebaselining actor {actor.Name} to ID {nextID} in database {database.name}";
                    newIDs[actor.id] = nextID;
                    actor.id = nextID;
                }
                nextID++;
            }
            // Apply new IDs:
            ApplyNewBaselineIDsToFields(newIDs, FieldType.Actor);
        }

        private void RebaselineItemIDs(DialogueDatabase database)
        {
            // Determine new IDs:
            var newIDs = new Dictionary<int, int>();
            var nextID = database.baseID;
            foreach (var item in database.items)
            {
                if (item.id != nextID)
                {
                    if (verbose) report += $"\nRebaselining item/quest {item.Name} to ID {nextID} in database {database.name}";
                    newIDs[item.id] = nextID;
                    item.id = nextID;
                }
                nextID++;
            }
            // Apply new IDs:
            ApplyNewBaselineIDsToFields(newIDs, FieldType.Item);
        }

        private void RebaselineLocationIDs(DialogueDatabase database)
        {
            // Determine new IDs:
            var newIDs = new Dictionary<int, int>();
            var nextID = database.baseID;
            foreach (var location in database.locations)
            {
                if (location.id != nextID)
                {
                    if (verbose) report += $"\nRebaselining location {location.Name} to ID {nextID} in database {database.name}";
                    newIDs[location.id] = nextID;
                    location.id = nextID;
                }
                nextID++;
            }
            // Apply new IDs:
            ApplyNewBaselineIDsToFields(newIDs, FieldType.Localization);
        }

        private void RebaselineVariableIDs(DialogueDatabase database)
        {
            // Determine new IDs:
            var newIDs = new Dictionary<int, int>();
            var nextID = database.baseID;
            foreach (var variable in database.variables)
            {
                if (variable.id != nextID)
                {
                    if (verbose) report += $"\nRebaselining variable {variable.Name} to ID {nextID} in database {database.name}";
                    newIDs[variable.id] = nextID;
                    variable.id = nextID;
                }
                nextID++;
            }
        }

        private void RebaselineConversationIDs(DialogueDatabase database)
        {
            // Determine new IDs:
            var newIDs = new Dictionary<int, int>();
            var nextID = database.baseID;
            foreach (var conversation in database.conversations)
            {
                if (conversation.id != nextID)
                {
                    if (verbose) report += $"\nRebaselining conversation {conversation.Title} to ID {nextID} in database {database.name}";
                    newIDs[conversation.id] = nextID;
                    conversation.id = nextID;
                }
                nextID++;
            }
            // Apply new IDs:
            foreach (var conversation in database.conversations)
            {
                foreach (var entry in conversation.dialogueEntries)
                {
                    foreach (var link in entry.outgoingLinks)
                    {
                        if (newIDs.TryGetValue(link.originConversationID, out var newOriginID))
                        {
                            link.originConversationID = newOriginID;
                        }
                        if (newIDs.TryGetValue(link.destinationConversationID, out var newDestinationID))
                        {
                            link.destinationConversationID = newDestinationID;
                        }
                    }
                }
            }
        }

        private void ApplyNewBaselineIDsToFields(Dictionary<int, int> newIDs, FieldType fieldType)
        {
            foreach (var database in prefs.databases)
            {
                foreach (var actor in database.actors)
                {
                    foreach (var field in actor.fields)
                    {
                        if (field.type == fieldType)
                        {
                            if (string.IsNullOrEmpty(field.value)) continue;
                            if (!int.TryParse(field.value, out var oldID))
                            {
                                report += $"\nInvalid {fieldType} ID found in actor {actor.Name} field {field.title} in database {database.name}";
                            }
                            else if (!newIDs.TryGetValue(oldID, out var newID))
                            {
                                report += $"\nError looking up new {fieldType} ID for actor {actor.Name} to update field {field.title} in database {database.name}";
                            }
                            else
                            {
                                field.value = newID.ToString();
                            }
                        }
                    }
                }
                foreach (var item in database.items)
                {
                    foreach (var field in item.fields)
                    {
                        if (field.type == fieldType)
                        {
                            if (string.IsNullOrEmpty(field.value)) continue;
                            if (!int.TryParse(field.value, out var oldID))
                            {
                                report += $"\nInvalid {fieldType} ID found in item/quest {item.Name} field {field.title} in database {database.name}";
                            }
                            else if (!newIDs.TryGetValue(oldID, out var newID))
                            {
                                report += $"\nError looking up new {fieldType} ID for item/quest {item.Name} to update field {field.title} in database {database.name}";
                            }
                            else
                            {
                                field.value = newID.ToString();
                            }
                        }
                    }
                }
                foreach (var location in database.locations)
                {
                    foreach (var field in location.fields)
                    {
                        if (field.type == fieldType)
                        {
                            if (string.IsNullOrEmpty(field.value)) continue;
                            if (!int.TryParse(field.value, out var oldID))
                            {
                                report += $"\nInvalid  {fieldType}  ID found in location {location.Name} field {field.title} in database {database.name}";
                            }
                            else if (!newIDs.TryGetValue(oldID, out var newID))
                            {
                                report += $"\nError looking up new {fieldType} ID for location {location.Name} to update field {field.title} in database {database.name}";
                            }
                            else
                            {
                                field.value = newID.ToString();
                            }
                        }
                    }
                }
                foreach (var variable in database.variables)
                {
                    foreach (var field in variable.fields)
                    {
                        if (field.type == fieldType)
                        {
                            if (string.IsNullOrEmpty(field.value)) continue;
                            if (!int.TryParse(field.value, out var oldID))
                            {
                                report += $"\nInvalid  {fieldType}  ID found in variable {variable.Name} field {field.title} in database {database.name}";
                            }
                            else if (!newIDs.TryGetValue(oldID, out var newID))
                            {
                                report += $"\nError looking up new {fieldType} ID for variable {variable.Name} to update field {field.title} in database {database.name}";
                            }
                            else
                            {
                                field.value = newID.ToString();
                            }
                        }
                    }
                }
                foreach (var conversation in database.conversations)
                {
                    foreach (var field in conversation.fields)
                    {
                        if (field.type == fieldType)
                        {
                            if (string.IsNullOrEmpty(field.value)) continue;
                            if (!int.TryParse(field.value, out var oldID))
                            {
                                report += $"\nInvalid {fieldType} ID found in conversation {conversation.Title} field {field.title} in database {database.name}";
                            }
                            else if (!newIDs.TryGetValue(oldID, out var newID))
                            {
                                if (fieldType != FieldType.Actor) // Conversations sometimes don't have Actor/Conversant assigned.
                                {
                                    report += $"\nError looking up new {fieldType} ID for conversation {conversation.Title} to update field {field.title} in database {database.name}";
                                }
                            }
                            else
                            {
                                field.value = newID.ToString();
                            }
                        }
                    }
                    foreach (var entry in conversation.dialogueEntries)
                    {
                        foreach (var field in entry.fields)
                        {
                            if (field.type == fieldType)
                            {
                                if (string.IsNullOrEmpty(field.value)) continue;
                                if (!int.TryParse(field.value, out var oldID))
                                {
                                    report += $"\nInvalid {fieldType} ID found in conversation {conversation.Title} entry {entry.id} field {field.title} in database {database.name}";
                                }
                                else if (!newIDs.TryGetValue(oldID, out var newID))
                                {
                                    if (fieldType != FieldType.Actor) // Conversations sometimes don't have Actor/Conversant assigned.
                                    {
                                        report += $"\nError looking up new {fieldType} ID for conversation {conversation.Title} entry {entry.id} to update field {field.title} in database {database.name}";
                                    }
                                }
                                else
                                {
                                    field.value = newID.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        private bool VerifyUniqueConversationIDs(DialogueDatabase database)
        {
            var result = true;
            HashSet<int> conversationIDs = new HashSet<int>();
            for (int i = 0; i < database.conversations.Count; i++)
            {
                var conversation = database.conversations[i];
                if (!conversationIDs.Contains(conversation.id))
                {
                    conversationIDs.Add(conversation.id);
                }
                else
                {
                    var s = "Unable to process conversations. In database '" + database +
                        "' two or more conversations have the same conversation ID " + conversation.id + ":";
                    for (int j = 0; j < database.conversations.Count; j++)
                    {
                        s += "\n" + database.conversations[j].Title;
                    }
                    Debug.LogError(s, database);
                    report += "\n" + s;
                    result = false;
                }
            }
            return result;
        }

        private void RecordActorConversantNamesPerDatabaseConversation(DialogueDatabase database, MasterIDs masterIDs)
        {
            var actorNamesByConversationTitle = new Dictionary<string, string>();
            var conversantNamesByConversationTitle = new Dictionary<string, string>();
            foreach (var conversation in database.conversations)
            {
                var conversationTitle = conversation.Title;
                var actor = database.GetActor(conversation.ActorID);
                var conversant = database.GetActor(conversation.ConversantID);
                actorNamesByConversationTitle[conversationTitle] = (actor != null) ? actor.Name : "";
                conversantNamesByConversationTitle[conversationTitle] = (conversant != null) ? conversant.Name : "";
            }
            masterIDs.actorNamesInConversations.Add(actorNamesByConversationTitle);
            masterIDs.conversantNamesInConversations.Add(conversantNamesByConversationTitle);
        }

        private void GetNewIDs(DialogueDatabase database, MasterIDs masterIDs)
        {
            if (verbose) report += "\nDetermining new IDs for database " + database.name;
            GetNewActorIDs(database, masterIDs);
            GetNewItemIDs(database, masterIDs);
            GetNewLocationIDs(database, masterIDs);
            GetNewVariableIDs(database, masterIDs);
        }

        private void ProcessDatabase(DialogueDatabase database, MasterIDs masterIDs, int i)
        {
            if (verbose) report += "\nConverting IDs in database " + database.name;
            ProcessConversations(database, masterIDs, i);
            ProcessActors(database, masterIDs);
            ProcessItems(database, masterIDs);
            ProcessLocations(database, masterIDs);
            ProcessVariables(database, masterIDs);
        }

        private void GetNewActorIDs(DialogueDatabase database, MasterIDs masterIDs)
        {
            foreach (var actor in database.actors)
            {
                if (masterIDs.actors.ContainsKey(actor.Name))
                {
                    // We've already found an actor with this name across our databases:
                    masterIDs.actors[actor.Name].oldID = actor.id;
                }
                else
                {
                    int newID;
                    if (!masterIDs.usedNewActorIDs.Contains(actor.id))//[TL] &&
                        //actor.id >= database.baseID)
                    {
                        // ID is unique so far and at least BaseID, so no need to assign new ID.
                        newID = actor.id;
                    }
                    else
                    {
                        // Find a new ID:
                        newID = template.GetNextActorID(database); // Try new ID from database.baseID.
                        if (masterIDs.usedNewActorIDs.Contains(newID))
                        {
                            // If can't get a unique ID from database.baseID, use highest ID + 1.
                            if (actor.id <= masterIDs.highestActorID)
                            {
                                masterIDs.highestActorID++;
                                newID = masterIDs.highestActorID;
                            }
                            else
                            {
                                newID = actor.id;
                            }
                        }
                    }
                    masterIDs.highestActorID = Mathf.Max(masterIDs.highestActorID, newID);
                    masterIDs.usedNewActorIDs.Add(newID);
                    masterIDs.actors.Add(actor.Name, new IDConversion(actor.id, newID));
                }
            }
        }

        private void GetNewItemIDs(DialogueDatabase database, MasterIDs masterIDs)
        {
            foreach (var item in database.items)
            {
                if (masterIDs.items.ContainsKey(item.Name))
                {
                    masterIDs.items[item.Name].oldID = item.id;
                }
                else
                {
                    int newID;
                    if (!masterIDs.usedNewItemIDs.Contains(item.id))//[TL] &&
                        //item.id >= database.baseID)
                    {
                        // ID is unique so far, so no need to assign new ID.
                        newID = item.id;
                    }
                    else
                    {
                        // Find a new ID:
                        newID = template.GetNextItemID(database); // Try new ID from database.baseID.
                        if (masterIDs.usedNewItemIDs.Contains(newID))
                        {
                            // If can't get a unique ID from database.baseID, use highest ID + 1.
                            if (item.id <= masterIDs.highestItemID)
                            {
                                masterIDs.highestItemID++;
                                newID = masterIDs.highestItemID;
                            }
                            else
                            {
                                newID = item.id;
                            }
                        }
                    }
                    masterIDs.highestItemID = Mathf.Max(masterIDs.highestItemID, newID);
                    masterIDs.usedNewItemIDs.Add(newID);
                    masterIDs.items.Add(item.Name, new IDConversion(item.id, newID));
                }
            }
        }

        private void GetNewLocationIDs(DialogueDatabase database, MasterIDs masterIDs)
        {
            foreach (var location in database.locations)
            {
                if (masterIDs.locations.ContainsKey(location.Name))
                {
                    masterIDs.locations[location.Name].oldID = location.id;
                }
                else
                {
                    int newID;
                    if (!masterIDs.usedNewLocationIDs.Contains(location.id))//[TL] &&
                        //location.id >= database.baseID)
                    {
                        // ID is unique so far, so no need to assign new ID.
                        newID = location.id;
                    }
                    else
                    {
                        // Find a new ID:
                        newID = template.GetNextLocationID(database); // Try new ID from database.baseID.
                        if (masterIDs.usedNewLocationIDs.Contains(newID))
                        {
                            // If can't get a unique ID from database.baseID, use highest ID + 1.
                            if (location.id <= masterIDs.highestLocationID)
                            {
                                masterIDs.highestLocationID++;
                                newID = masterIDs.highestLocationID;
                            }
                            else
                            {
                                newID = location.id;
                            }
                        }
                    }
                    masterIDs.highestLocationID = Mathf.Max(masterIDs.highestLocationID, newID);
                    masterIDs.usedNewLocationIDs.Add(newID);
                    masterIDs.locations.Add(location.Name, new IDConversion(location.id, newID));
                }
            }
        }

        private void GetNewVariableIDs(DialogueDatabase database, MasterIDs masterIDs)
        {
            foreach (var variable in database.variables)
            {
                if (masterIDs.variables.ContainsKey(variable.Name))
                {
                    masterIDs.variables[variable.Name].oldID = variable.id;
                }
                else
                {
                    int newID;
                    if (!masterIDs.usedNewVariableIDs.Contains(variable.id))//[TL] &&
                        //variable.id >= database.baseID)
                    {
                        // ID is unique so far, so no need to assign new ID.
                        newID = variable.id;
                    }
                    else
                    {
                        // Find a new ID:
                        newID = template.GetNextVariableID(database); // Try new ID from database.baseID.
                        if (masterIDs.usedNewVariableIDs.Contains(newID))
                        {
                            // If can't get a unique ID from database.baseID, use highest ID + 1.
                            if (variable.id <= masterIDs.highestVariableID)
                            {
                                masterIDs.highestVariableID++;
                                newID = masterIDs.highestVariableID;
                            }
                            else
                            {
                                newID = variable.id;
                            }
                        }
                    }
                    masterIDs.highestVariableID = Mathf.Max(masterIDs.highestVariableID, newID);
                    masterIDs.usedNewVariableIDs.Add(newID);
                    masterIDs.variables.Add(variable.Name, new IDConversion(variable.id, newID));
                }
            }
        }

        private void ProcessFieldIDs(DialogueDatabase database, List<Field> fields, MasterIDs masterIDs)
        {
            foreach (var field in fields)
            {
                int oldID = Tools.StringToInt(field.value);
                switch (field.type)
                {
                    case FieldType.Actor:
                        Actor actor = database.GetActor(oldID);
                        if (actor != null) field.value = FindIDConversion(actor.Name, masterIDs.actors, oldID).ToString();
                        break;
                    case FieldType.Item:
                        Item item = database.GetItem(oldID);
                        if (item != null) field.value = FindIDConversion(item.Name, masterIDs.items, oldID).ToString();
                        break;
                    case FieldType.Location:
                        Location location = database.GetLocation(oldID);
                        if (location != null) field.value = FindIDConversion(location.Name, masterIDs.locations, oldID).ToString();
                        break;
                }
            }
        }

        private int FindIDConversion(string name, Dictionary<string, IDConversion> dict, int oldID)
        {
            if (dict.ContainsKey(name))
            {
                return dict[name].newID;
            }
            else
            {
                var s = "Warning: No ID conversion entry found for " + name;
                Debug.LogWarning(s);
                report += "\n" + s;
                return oldID;
            }
        }

        private void ProcessActors(DialogueDatabase database, MasterIDs masterIDs)
        {
            foreach (var actor in database.actors)
            {
                int newID = FindIDConversion(actor.Name, masterIDs.actors, actor.id);
                if (newID != actor.id)
                {
                    if (verbose) report += string.Format("\nActor {0}: ID [{1}]-->[{2}] in {3}", actor.Name, actor.id, newID, database.name);
                    actor.id = newID;
                }
                ProcessFieldIDs(database, actor.fields, masterIDs);
            }
        }

        private void ProcessItems(DialogueDatabase database, MasterIDs masterIDs)
        {
            foreach (var item in database.items)
            {
                int newID = FindIDConversion(item.Name, masterIDs.items, item.id);
                if (newID != item.id)
                {
                    if (verbose) report += string.Format("\nItem {0}: ID [{1}]-->[{2}] in {3}", item.Name, item.id, newID, database.name);
                    item.id = newID;
                }
                ProcessFieldIDs(database, item.fields, masterIDs);
            }
        }

        private void ProcessLocations(DialogueDatabase database, MasterIDs masterIDs)
        {
            foreach (var location in database.locations)
            {
                int newID = FindIDConversion(location.Name, masterIDs.locations, location.id);
                if (newID != location.id)
                {
                    if (verbose) report += string.Format("\nLocation {0}: ID [{1}]-->[{2}] in {3}", location.Name, location.id, newID, database.name);
                    location.id = newID;
                }
                ProcessFieldIDs(database, location.fields, masterIDs);
            }
        }

        private void ProcessVariables(DialogueDatabase database, MasterIDs masterIDs)
        {
            foreach (var variable in database.variables)
            {
                int newID = FindIDConversion(variable.Name, masterIDs.variables, variable.id);
                if (newID != variable.id)
                {
                    if (verbose) report += string.Format("\nVariable {0}: ID [{1}]-->[{2}] in {3}", variable.Name, variable.id, newID, database.name);
                    variable.id = newID;
                }
                ProcessFieldIDs(database, variable.fields, masterIDs);
            }
        }

        private void ProcessConversations(DialogueDatabase database, MasterIDs masterIDs, int i)
        {
            Dictionary<int, int> newIDs = GetNewConversationIDs(database, masterIDs);
            foreach (var conversation in database.conversations)
            {
                if (newIDs.ContainsKey(conversation.id))
                {
                    if (verbose) report += string.Format("\nConversation '{0}': ID [{1}]-->[{2}] in {3}", conversation.Title, conversation.id, newIDs[conversation.id], database.name);
                    conversation.id = newIDs[conversation.id];
                    ProcessFieldIDs(database, conversation.fields, masterIDs);
                    foreach (DialogueEntry entry in conversation.dialogueEntries)
                    {
                        entry.conversationID = conversation.id;
                        ProcessFieldIDs(database, entry.fields, masterIDs);
                        foreach (var link in entry.outgoingLinks)
                        {
                            if (newIDs.ContainsKey(link.originConversationID)) link.originConversationID = newIDs[link.originConversationID];
                            if (newIDs.ContainsKey(link.destinationConversationID)) link.destinationConversationID = newIDs[link.destinationConversationID];
                        }
                    }

                    // Verify actor & conversant:
                    var conversationTitle = conversation.Title;
                    var actor = database.GetActor(conversation.ActorID);
                    var conversant = database.GetActor(conversation.ConversantID);
                    var actorName = (actor != null) ? actor.Name : "";
                    var conversantName = (conversant != null) ? conversant.Name : "";
                    var originalActorName = masterIDs.actorNamesInConversations[i][conversationTitle];
                    var originalConversantName = masterIDs.conversantNamesInConversations[i][conversationTitle];
                    if (actorName != originalActorName)
                    {
                        if (verbose) report += $"\nFixing actor {originalActorName} in {database.name}:{conversationTitle}";
                        var intendedActor = database.GetActor(originalActorName);
                        if (intendedActor != null) conversation.ActorID = intendedActor.id;
                    }
                    if (conversantName != originalConversantName)
                    {
                        if (verbose) report += $"\nFixing conversant {originalConversantName} in {database.name}:{conversationTitle}";
                        var intendedConversant = database.GetActor(originalConversantName);
                        if (intendedConversant != null) conversation.ConversantID = intendedConversant.id;
                    }
                }
            }
        }

        private Dictionary<int, int> GetNewConversationIDs(DialogueDatabase database, MasterIDs masterIDs)
        {
            Dictionary<int, int> newIDs = new Dictionary<int, int>();
            foreach (var conversation in database.conversations)
            {
                if (!masterIDs.usedNewConversationIDs.Contains(conversation.id))
                {
                    // ID is unique so far, so no need to assign new ID.
                    masterIDs.usedNewConversationIDs.Add(conversation.id);
                    masterIDs.highestConversationID = Mathf.Max(masterIDs.highestConversationID, conversation.id);
                }
                else
                {
                    // Find a new ID:
                    int newID;
                    newID = template.GetNextConversationID(database); // Try new ID from database.baseID.
                    if (masterIDs.usedNewConversationIDs.Contains(newID))
                    {
                        // If can't get a unique ID from database.baseID, use highest ID + 1.
                        masterIDs.highestConversationID++;
                        newID = masterIDs.highestConversationID;
                        if (newIDs.ContainsKey(conversation.id))
                        {
                            var s = "Unique ID Tool error: In '" + database + "' more than one conversation uses ID " + conversation.id + ".";
                            Debug.LogError(s, database);
                            report += "\n" + s;
                            newID = conversation.id;
                        }
                    }
                    masterIDs.highestConversationID = Mathf.Max(masterIDs.highestConversationID, newID);
                    masterIDs.usedNewConversationIDs.Add(newID);
                    newIDs.Add(conversation.id, newID);
                }
            }
            return newIDs;
        }

    }

}
