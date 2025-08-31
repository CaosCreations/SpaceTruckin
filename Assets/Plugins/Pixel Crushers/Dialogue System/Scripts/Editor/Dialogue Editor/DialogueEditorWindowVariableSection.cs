// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System;

namespace PixelCrushers.DialogueSystem.DialogueEditor
{

    /// <summary>
    /// This part of the Dialogue Editor window handles the Variables tab. Drawing is now
    /// handled by a DialogueEditorVariableView.
    /// </summary>
    public partial class DialogueEditorWindow
    {
        [SerializeField]
        private DialogueEditorVariableView variableView;

        private HashSet<int> syncedVariableIDs => variableView != null ? variableView.syncedVariableIDs : null;
        private AssetFoldouts variableFoldouts = new AssetFoldouts()
        {
            fields = new Dictionary<int, bool>() { { 0, true } }
        };

        private void ResetVariableSection()
        {
            CleanUpUnnamedVariables();
            variableView = new DialogueEditorVariableView();
            variableView.Initialize(database, template, true);
        }

        private void CleanUpUnnamedVariables()
        {
            if (database == null) return;
            var originalCount = database.variables.Count;
            database.variables.RemoveAll(x => string.IsNullOrEmpty(x.Name));
            var numRemoved = originalCount - database.variables.Count;
            if (numRemoved > 0)
            {
                Debug.Log($"Dialogue System: Removed {numRemoved} variables with blank names.");
            }
        }

        public void RefreshVariableView()
        {
            variableView.RefreshView();
            Repaint();
        }

        private void DrawVariableSection()
        {
            if (variableView == null || (variableView.database == null && database != null))
            {
                ResetVariableSection();
            }
            variableView.Draw();
            if (Application.isPlaying)
            {
                GUI.Label(new Rect(72, -4, 500, 30), "(Use Watches tab or Variable Viewer for runtime values.)");
            }
        }

        public void DrawSelectedVariableSecondPart()
        {
            var variable = inspectorSelection as Variable;
            if (variable == null) return;
            DrawOtherVariablePrimaryFields(variable);
            DrawFieldsFoldout<Variable>(variable, 0, variableFoldouts);
            DrawAssetSpecificPropertiesSecondPart(variable, 0, variableFoldouts);
        }

        private void DrawOtherVariablePrimaryFields(Variable variable)
        {
            if (variable == null || variable.fields == null || template.variablePrimaryFieldTitles == null) return;
            foreach (var field in variable.fields)
            {
                var fieldTitle = field.title;
                if (string.IsNullOrEmpty(fieldTitle)) continue;
                if (!template.variablePrimaryFieldTitles.Contains(field.title)) continue;
                DrawMainSectionField(field);
            }
        }

        public bool IsVariableSyncedFromOtherDB(Variable variable)
        {
            return variable != null && syncedVariableIDs != null && syncedVariableIDs.Contains(variable.id);
        }

    }

}