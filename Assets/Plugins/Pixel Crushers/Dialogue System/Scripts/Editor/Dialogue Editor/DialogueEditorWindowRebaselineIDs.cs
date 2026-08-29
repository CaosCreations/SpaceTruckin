// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.DialogueEditor
{

    /// <summary>
    /// This part of the Dialogue Editor window handles rebaselining internal database IDs.
    /// </summary>
    public partial class DialogueEditorWindow
    {

        private void ConfirmRebaselineAllIDs()
        {
            if (database == null) return;
            if (EditorUtility.DisplayDialog("Rebaseline All IDs",
                $"This will reassign new internal IDs to all content in this database and any databases connected via Sync From DB, counting up from each database's Base ID.\n\nContinue?", "OK", "Cancel"))
            {
                RebaselineAllIDs();
            }
        }

        private void RebaselineAllIDs()
        {
            var originalDatabase = database;
            try
            {
                var syncTargets = FindSyncTargets();
                var databasesToRebaseline = FindDatabasesToRebaseline(syncTargets);
                RebaselineAllIDsInDatabases(databasesToRebaseline, syncTargets);
                SetRebasedlinedIDsInDatabasesPositive(databasesToRebaseline);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                database = originalDatabase;
                Reset();
            }
        }

        private Dictionary<DialogueDatabase, List<DialogueDatabase>> FindSyncTargets() //<source, <targets>>
        {
            // Create a list of databases and any targets they that sync their content into:
            var syncTargets = new Dictionary<DialogueDatabase, List<DialogueDatabase>>();
            var allDatabaseGuids = AssetDatabase.FindAssets("t:DialogueDatabase");
            for (int i = 0; i < allDatabaseGuids.Length; i++)
            {
                var progress = (float)i / (float)allDatabaseGuids.Length;
                var guid = allDatabaseGuids[i];
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var database = AssetDatabase.LoadAssetAtPath<DialogueDatabase>(path);
                if (database == null)
                {
                    Debug.LogWarning($"Unable to load database with GUID {guid} at path {path}.");
                    continue;
                }
                EditorUtility.DisplayProgressBar("Rebaselining IDs", $"Identifying all connected databases.\nChecking {database.name}", progress);
                if (database.syncInfo.syncActors) AddSyncTarget(syncTargets, database.syncInfo.syncActorsDatabase, database);
                if (database.syncInfo.syncItems) AddSyncTarget(syncTargets, database.syncInfo.syncItemsDatabase, database);
                if (database.syncInfo.syncLocations) AddSyncTarget(syncTargets, database.syncInfo.syncLocationsDatabase, database);
                if (database.syncInfo.syncVariables) AddSyncTarget(syncTargets, database.syncInfo.syncVariablesDatabase, database);
            }
            return syncTargets;
        }

        private void AddSyncTarget(Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets,
            DialogueDatabase source, DialogueDatabase target)
        {
            if (!syncTargets.TryGetValue(source, out var targets))
            {
                targets = new List<DialogueDatabase>();
                syncTargets.Add(source, targets);
            }
            if (!targets.Contains(target)) targets.Add(target);
        }

        private List<DialogueDatabase> FindDatabasesToRebaseline(Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
        {
            // syncTargets is essentially a graph of source databases pointing to target databases that
            // they sync into. Return the part of the graph containing the current database. This is the
            // complete list of all databases connected in any way to the current database.
            var hashSet = new HashSet<DialogueDatabase> { database };
            foreach (var kvp in syncTargets)
            {
                var source = kvp.Key;
                var targets = kvp.Value;
                if (hashSet.Contains(source))
                {
                    AddToRelevantSyncDatabases(hashSet, source, targets);
                }
                foreach (var target in targets)
                {
                    if (hashSet.Contains(target))
                    {
                        AddToRelevantSyncDatabases(hashSet, source, targets);
                    }
                }
            }
            var relevantDatabases = new List<DialogueDatabase>(hashSet);
            relevantDatabases.Sort((x, y) => y.baseID.CompareTo(x.baseID)); // Sort by descending BaseID values.
            return relevantDatabases;
        }

        private void AddToRelevantSyncDatabases(HashSet<DialogueDatabase> hashSet, DialogueDatabase source, List<DialogueDatabase> targets)
        {
            hashSet.Add(source);
            foreach (var target in targets)
            {
                hashSet.Add(target);
            }
        }

        private void RebaselineAllIDsInDatabases(List<DialogueDatabase> databasesToRebaseline,
            Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
        {
            foreach (var database in databasesToRebaseline)
            {
                RebaselineAllIDsInDatabase(database, syncTargets);
            }
        }

        private void RebaselineAllIDsInDatabase(DialogueDatabase database,
            Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
        {
            try
            {
                this.database = database;
                Reset();
                // Record synced content so we know not to change their IDs. (Sync source will change them.)
                RecordAllSyncedContent(); 
                Undo.RegisterCompleteObjectUndo(database, "Dialogue Database");

                // Rebaseline IDs but set them to their negated values for now in pass 1
                // to avoid conflicts with assets that are already ising the new IDs.
                // Once all databases have been processed, we'll set negated values positive.
                RebaselineActorIDs(syncTargets);
                RebaselineItemIDs(syncTargets);
                RebaselineLocationIDs(syncTargets);
                RebaselineVariableIDs(syncTargets);
                RebaselineConversationIDs(syncTargets);
                Debug.Log($"Rebaselined all IDs in {database} counting up from Base ID {database.baseID}.", database);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                Undo.PerformUndo();
            }
        }

        private void RecordAllSyncedContent()
        {
            RecordSyncedActorIDs();
            RecordSyncedItemIDs();
            RecordSyncedLocationIDs();
            variableView.RecordSyncedVariableIDs();
        }

        private void RebaselineActorIDs(Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
        {
            EditorUtility.DisplayProgressBar("Rebaselining IDs", "Actor IDs", 0.20f);
            RebaselineAssetIDs<Actor>(database.actors, syncedActorIDs, FieldType.Actor, syncTargets);
        }

        private void RebaselineItemIDs(Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
        {
            EditorUtility.DisplayProgressBar("Rebaselining IDs", "Item IDs", 0.40f);
            RebaselineAssetIDs<Item>(database.items, syncedItemIDs, FieldType.Item, syncTargets);
        }

        private void RebaselineLocationIDs(Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
        {
            EditorUtility.DisplayProgressBar("Rebaselining IDs", "Location IDs", 0.50f);
            RebaselineAssetIDs<Location>(database.locations, syncedLocationIDs, FieldType.Location, syncTargets);
        }

        private void RebaselineVariableIDs(Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
        {
            EditorUtility.DisplayProgressBar("Rebaselining IDs", "Variable IDs", 0.60f);
            RebaselineAssetIDs<Variable>(database.variables, syncedVariableIDs, FieldType.Text, syncTargets);
        }

        private void RebaselineConversationIDs(Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
        {
            EditorUtility.DisplayProgressBar("Rebaselining IDs", "Conversation IDs", 0.80f);
            RebaselineAssetIDs<Conversation>(database.conversations, null, FieldType.Text, syncTargets);
        }

        private void RebaselineAssetIDs<T>(List<T> assets, HashSet<int> syncedAssets, FieldType fieldType,
            Dictionary<DialogueDatabase, List<DialogueDatabase>> syncTargets)
            where T : Asset
        {
            // Get the list of target databases that this database syncs into:
            if (!syncTargets.TryGetValue(database, out var targets)) targets = new List<DialogueDatabase>();

            // Determine new IDs:
            int nextID = database.baseID;
            var newIDs = new Dictionary<int, int>();
            foreach (var asset in assets)
            {
                if (syncedAssets != null && syncedAssets.Contains(asset.id)) continue; // Don't touch assets synced from another database.
                int oldID = asset.id;
                int newID = -nextID; // Use negated value for now so we don't conflict with assets that already use ID.
                nextID++;
                newIDs[oldID] = newID;
                ChangeAssetID<T>(database, assets, oldID, newID, fieldType);

                // Update the ID in target databases:
                foreach (var target in targets)
                {
                    if (typeof(T) == typeof(Actor) && target.syncInfo.syncActors)
                    {
                        var targetAsset = target.actors.Find(x => x.id == oldID);
                        if (targetAsset != null)
                        {
                            ChangeAssetID<Actor>(target, target.actors, oldID, newID, fieldType);
                        }
                    }
                    else if (typeof(T) == typeof(Item) && target.syncInfo.syncItems)
                    {
                        var targetAsset = target.items.Find(x => x.id == oldID);
                        if (targetAsset != null)
                        {
                            ChangeAssetID<Item>(target, target.items, oldID, newID, fieldType);
                        }
                    }
                    else if (typeof(T) == typeof(Location) && target.syncInfo.syncLocations)
                    {
                        var targetAsset = target.locations.Find(x => x.id == oldID);
                        if (targetAsset != null)
                        {
                            ChangeAssetID<Location>(target, target.locations, oldID, newID, fieldType);
                        }
                    }
                    else if (typeof(T) == typeof(Variable) && target.syncInfo.syncVariables)
                    {
                        var targetAsset = target.variables.Find(x => x.id == oldID);
                        if (targetAsset != null)
                        {
                            ChangeAssetID<Variable>(target, target.variables, oldID, newID, fieldType);
                        }
                    }                    
                }
            }
        }

        private void ChangeAssetID<T>(DialogueDatabase database, List<T> assets, int oldID, int newID, FieldType fieldType)
            where T : Asset
        {
            var asset = assets.Find(x => x.id == oldID);
            asset.id = newID;

            // Update references to ID in other assets in database:
            if (fieldType == FieldType.Actor || fieldType == FieldType.Item || fieldType == FieldType.Location)
            {
                // Update references to assets in fields:
                SetNewAssetIDReferences<Actor>(oldID, newID, database.actors, fieldType);
                SetNewAssetIDReferences<Item>(oldID, newID, database.items, fieldType);
                SetNewAssetIDReferences<Location>(oldID, newID, database.locations, fieldType);
                SetNewAssetIDReferences<Conversation>(oldID, newID, database.conversations, fieldType);
                var fieldTypeName = fieldType.ToString();
                foreach (var conversation in database.conversations)
                {
                    foreach (var entry in conversation.dialogueEntries)
                    {
                        SetNewAssetIDReferencesInFields(oldID, newID, entry.fields, fieldType, fieldTypeName);
                    }
                }
            }
            else if (typeof(T) == typeof(Conversation))
            {
                // Update dialogue entries and links:
                foreach (var conversation in database.conversations)
                {
                    foreach (var entry in conversation.dialogueEntries)
                    {
                        if (entry.conversationID == oldID) entry.conversationID = newID;
                        foreach (var link in entry.outgoingLinks)
                        {
                            if (link.originConversationID == oldID) link.originConversationID = newID;
                            if (link.destinationConversationID == oldID) link.destinationConversationID = newID;
                        }
                    }
                }
            }

        }

        private void SetNewAssetIDReferences<T>(int oldID, int newID, List<T> assets, FieldType fieldType)
            where T : Asset
        {
            var fieldTypeName = fieldType.ToString();
            foreach (var asset in assets)
            {
                SetNewAssetIDReferencesInFields(oldID, newID, asset.fields, fieldType, fieldTypeName);
            }
        }

        private void SetNewAssetIDReferencesInFields(int oldID, int newID, List<Field> fields, FieldType fieldType, string fieldTypeName)
        {
            foreach (var field in fields)
            {
                if (field.type == fieldType || 
                    (!string.IsNullOrEmpty(field.typeString) && field.typeString.EndsWith(fieldTypeName)))
                {
                    if (int.TryParse(field.value, out var myOldID) && myOldID == oldID)
                    {
                        field.value = newID.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        field.type = fieldType;
                    }
                }
            }
        }

        // In pass 1, we set new IDs to their negated values so they didn't conflict with any
        // existing assets already using the IDs. Now in pass 2 we set those IDs positive
        // since all conflicts have been avoided.
        private void SetRebasedlinedIDsInDatabasesPositive(List<DialogueDatabase> databases)
        {
            foreach (var database in databases)
            {
                this.database = database;
                Reset();
                SetRebaselinedIDsPositiveInDatabase();
            }
        }

        private void SetRebaselinedIDsPositiveInDatabase()
        {
            EditorUtility.DisplayProgressBar("Rebaselining IDs", "Finishing ID assignments", 0.90f);
            database.actors.ForEach(x => SetRebaselinedIDsPositiveInAsset<Actor>(x));
            database.items.ForEach(x => SetRebaselinedIDsPositiveInAsset<Item>(x));
            database.locations.ForEach(x => SetRebaselinedIDsPositiveInAsset<Location>(x));
            database.variables.ForEach(x => SetRebaselinedIDsPositiveInAsset<Variable>(x));
            foreach (var conversation in database.conversations)
            {
                SetRebaselinedIDsPositiveInAsset<Conversation>(conversation);
                foreach (var entry in conversation.dialogueEntries)
                {
                    if (entry.conversationID < 0) entry.conversationID = -entry.conversationID;
                    SetRebaselinedIDsPositiveInFields(entry.fields);
                    foreach (var link in entry.outgoingLinks)
                    {
                        if (link.originConversationID < 0) link.originConversationID = -link.originConversationID;
                        if (link.destinationConversationID < 0) link.destinationConversationID = -link.destinationConversationID;
                    }
                }
            }
        }

        private void SetRebaselinedIDsPositiveInAsset<T>(Asset asset) where T : Asset
        {
            if (asset.id < 0) asset.id = -asset.id;
            SetRebaselinedIDsPositiveInFields(asset.fields);
        }

        private void SetRebaselinedIDsPositiveInFields(List<Field> fields)
        {
            foreach (var field in fields)
            {
                if (field.type == FieldType.Actor || field.type == FieldType.Item || field.type == FieldType.Location)
                {
                    if (int.TryParse(field.value, out var idValue) && idValue < 0)
                    {
                        field.value = (-idValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
        }

    }

}
